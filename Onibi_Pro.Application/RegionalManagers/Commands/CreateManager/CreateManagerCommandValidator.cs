using FluentValidation;

namespace Onibi_Pro.Application.RegionalManagers.Commands.CreateManager;
public sealed class CreateManagerCommandValidator : AbstractValidator<CreateManagerCommand>
{
    public CreateManagerCommandValidator()
    {
        RuleFor(x => x.RestaurantId.Value).NotEqual(Guid.Empty);
    }
}
