using FluentValidation;

namespace Onibi_Pro.Application.Authentication.Commands;
public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotNull();
        RuleFor(x => x.Password).NotNull();
        RuleFor(x => x.FirstName).NotNull();
        RuleFor(x => x.LastName).NotNull();
    }
}
