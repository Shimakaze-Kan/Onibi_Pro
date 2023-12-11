using FluentValidation;

using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
public sealed class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    public CreateRestaurantCommandValidator()
    {
        RuleForEach(x => x.Employees).SetValidator(new EmployeeCommandValidator());
    }

    private class EmployeeCommandValidator : AbstractValidator<EmployeeCommand>
    {
        public EmployeeCommandValidator()
        {
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleForEach(x => x.EmployeePositions).SetValidator(new EmployeePositionCommandValidator());
        }
    }

    private class EmployeePositionCommandValidator : AbstractValidator<EmployeePositionCommand>
    {
        public EmployeePositionCommandValidator()
        {
            RuleFor(x => x.Position).Must(BeValidEnum<Positions>).WithMessage("Invalid Position");
        }

        private static bool BeValidEnum<TEnum>(string value) where TEnum : struct
        {
            return Enum.TryParse<TEnum>(value, out _);
        }
    }
}
