using ErrorOr;
using FluentValidation;
using MediatR;
using SleekFlow.Todo.Application.Auths.Dtos;
using SleekFlow.Todo.Application.Common.Interfaces;

namespace SleekFlow.Todo.Application.Auths.Commands;

public sealed record UserRegister(
	string UserName, 
	string Password) : IRequest<ErrorOr<TokenResponseDto>>;

internal sealed class UserRegisterValidator : AbstractValidator<UserRegister>
{
    public UserRegisterValidator()
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

internal sealed class UserRegisterHandler : IRequestHandler<UserRegister, ErrorOr<TokenResponseDto>>
{
	private readonly IAuthService _authService;

	public UserRegisterHandler(IAuthService authService)
		=> _authService = authService;

    public async Task<ErrorOr<TokenResponseDto>> Handle(UserRegister request, CancellationToken cancellationToken)
	{
		ErrorOr<string> result = await _authService.CreateUserAsync(request.UserName, request.Password, cancellationToken);

		return result.IsError ? result.FirstError : new TokenResponseDto(result.Value);
	}
}
