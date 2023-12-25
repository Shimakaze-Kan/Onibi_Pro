using FluentValidation;

namespace Onibi_Pro.Application.Authentication.Queries;
public sealed class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email).NotNull();
        RuleFor(x => x.Password).NotNull();
    }
}
