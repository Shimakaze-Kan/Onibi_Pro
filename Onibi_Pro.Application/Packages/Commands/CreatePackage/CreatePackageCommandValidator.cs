using FluentValidation;

namespace Onibi_Pro.Application.Packages.Commands.CreatePackage;
public class CreatePackageCommandValidator : AbstractValidator<CreatePackageCommand>
{
    public CreatePackageCommandValidator()
    {
        RuleFor(x => x.Message).MaximumLength(255);
    }
}
