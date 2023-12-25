using FluentValidation;

using Onibi_Pro.Domain.RestaurantAggregate.Entities;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateSchedule;
public sealed class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
{
    public CreateScheduleCommandValidator()
    {
        RuleFor(x => x.Title).NotNull();
        RuleFor(x => x.Priority).NotNull().NotEmpty().Must(x => Enum.TryParse<Priorities>(x, ignoreCase: true, out _));
        RuleFor(x => x.EmployeeIds).NotNull();
    }
}
