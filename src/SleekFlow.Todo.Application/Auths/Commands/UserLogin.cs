using ErrorOr;
using FluentValidation;
using MediatR;
using SleekFlow.Todo.Application.Auths.Dtos;
using SleekFlow.Todo.Application.Common.Interfaces;

namespace SleekFlow.Todo.Application.Auths.Commands;

public sealed record UserLogin(
	string UserName,
	string Password) : IRequest<ErrorOr<TokenResponseDto>>;

internal sealed class UserLoginValidator : AbstractValidator<UserLogin>
{
	public UserLoginValidator()
	{
		ClassLevelCascadeMode = CascadeMode.Stop;

		RuleFor(x => x.UserName)
			.NotEmpty()
				.WithMessage("{PropertyName} is required.");

		RuleFor(x => x.Password)
			.NotEmpty()
				.WithMessage("{PropertyName} is required.");
	}
}

internal sealed class UserLoginHandler : IRequestHandler<UserLogin, ErrorOr<TokenResponseDto>>
{
	private readonly IAuthService _authService;

	public UserLoginHandler(IAuthService authService)
		=> _authService = authService;

	public async Task<ErrorOr<TokenResponseDto>> Handle(UserLogin request, CancellationToken cancellationToken)
	{
		ErrorOr<string> result = await _authService.VerifyCredentialsAsync(request.UserName, request.Password, cancellationToken);

		return result.IsError ? result.FirstError : new TokenResponseDto(result.Value);
	}
}