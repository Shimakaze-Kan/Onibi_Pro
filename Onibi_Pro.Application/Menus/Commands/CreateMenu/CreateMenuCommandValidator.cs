using FluentValidation;

using Onibi_Pro.Domain.Common.ValueObjects;

namespace Onibi_Pro.Application.Menus.Commands.CreateMenu;

public sealed class CreateMenuCommandValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.MenuItems).NotEmpty();
        RuleForEach(x => x.MenuItems).SetValidator(new MenuItemCommandValidator());
    }

    private class MenuItemCommandValidator : AbstractValidator<MenuItemCommand>
    {
        public MenuItemCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.Ingredients).NotEmpty();
            RuleForEach(x => x.Ingredients).SetValidator(new IngredientCommandValidator());
        }
    }

    private class IngredientCommandValidator : AbstractValidator<IngredientCommand>
    {
        public IngredientCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Unit).NotEmpty().Must(BeValidEnum<UnitType>).WithMessage("Invalid Unit");
            RuleFor(x => x.Quantity).GreaterThan(0);
        }

        private static bool BeValidEnum<TEnum>(string value)
        {
            return Enum.TryParse<UnitType>(value, out _);
        }
    }
}
