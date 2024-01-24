using FluentValidation;

namespace Onibi_Pro.Application.Authentication.Commands;
public sealed class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty().NotNull();
    }
}
