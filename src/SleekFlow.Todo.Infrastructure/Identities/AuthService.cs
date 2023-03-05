using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SleekFlow.Todo.Application.Common.Exceptions;
using SleekFlow.Todo.Application.Common.Interfaces;
using SleekFlow.Todo.Infrastructure.Persistence;
using System.Security.Claims;
using static SleekFlow.Todo.Domain.Constants.DomainConstants;

namespace SleekFlow.Todo.Infrastructure.Identities;

internal sealed class AuthService : IAuthService
{
	private readonly TodoDbContext _dbContext;
	private readonly IPasswordHasher<AppUser> _passwordHasher;
	private readonly ITokenService _tokenService;

	public AuthService(
		TodoDbContext dbContext,
		IPasswordHasher<AppUser> passwordHasher,
		ITokenService tokenService)
	{
		_dbContext = dbContext;
		_passwordHasher = passwordHasher;
		_tokenService = tokenService;
	}

	public async Task<ErrorOr<string>> CreateUserAsync(string username, string password, CancellationToken cancellationToken)
	{
		AppUser? user = await _dbContext
			.Users
			.FirstOrDefaultAsync(x => x.UserName.Equals(username), cancellationToken);

		if (user is not null)
		{
			return Errors.AuthErrors.UserAlreadyExists(user.UserName);
		}

		user = AppUser.Create(username, password, _passwordHasher);

		_dbContext.Users.Add(user);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return _tokenService.CreateToken(CreateClaims(user));
	}

	public async Task<ErrorOr<string>> VerifyCredentialsAsync(string username, string password, CancellationToken cancellationToken)
	{
		AppUser? user = await _dbContext
			.Users
			.FirstOrDefaultAsync(x => x.UserName.Equals(username), cancellationToken);

		if (user is null ||
			_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) is PasswordVerificationResult.Failed)
		{
			return Errors.AuthErrors.LoginFailure;
		}

		return _tokenService.CreateToken(CreateClaims(user));
	}

	public static IEnumerable<Claim> CreateClaims(AppUser appUser)
		=> new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
			new Claim(ClaimTypes.Name, appUser.UserName.ToString())
		};
}