using Microsoft.AspNetCore.Identity;
using SleekFlow.Todo.Domain.Common;
using static SleekFlow.Todo.Domain.Constants.DomainConstants;

namespace SleekFlow.Todo.Infrastructure.Identities;

internal class AppUser : AuditableEntity
{
	public Guid Id { get; private set; }

	public string UserName { get; private set; } = null!;

	public string PasswordHash { get; private set; } = null!;

	public static AppUser Create(string userName, string password, IPasswordHasher<AppUser> passwordHasher)
	{
		AppUser newUser = new()
		{
			Id = Guid.NewGuid(),
			UserName = userName
		};

		newUser.CreatedBy = newUser.Id.ToString();
		string hashedPassword = passwordHasher.HashPassword(newUser, password);

		newUser.UpdatePasswordHash(hashedPassword);

		return newUser;
	}

	public string UpdatePasswordHash(string passwordHash)
		=> PasswordHash = passwordHash;
}