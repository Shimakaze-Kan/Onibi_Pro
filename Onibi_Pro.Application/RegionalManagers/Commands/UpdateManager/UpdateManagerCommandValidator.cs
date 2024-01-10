using FluentValidation;

namespace Onibi_Pro.Application.RegionalManagers.Commands.UpdateManager;
public sealed class UpdateManagerCommandValidator : AbstractValidator<UpdateManagerCommand>
{
    public UpdateManagerCommandValidator()
    {
        RuleFor(x => x.ManagerId.Value).NotEqual(Guid.Empty);
        RuleFor(x => x.RestaurantId.Value).NotEqual(Guid.Empty);
    }
}
