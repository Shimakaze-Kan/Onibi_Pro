using FluentValidation;

using Onibi_Pro.Domain.RestaurantAggregate.Entities;

namespace Onibi_Pro.Application.Restaurants.Commands.EditSchedule;
public sealed class EditScheduleCommandValidator : AbstractValidator<EditScheduleCommand>
{
    public EditScheduleCommandValidator()
    {
        RuleFor(x => x.Title).NotNull();
        RuleFor(x => x.Priority).NotNull().NotEmpty().Must(x => Enum.TryParse<Priorities>(x, ignoreCase: true, out _));
        RuleFor(x => x.EmployeeIds).NotNull();
    }
}
