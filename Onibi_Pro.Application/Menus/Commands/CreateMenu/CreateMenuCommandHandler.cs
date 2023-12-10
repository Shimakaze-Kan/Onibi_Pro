using ErrorOr;
using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.MenuAggregate;
using Onibi_Pro.Domain.MenuAggregate.Entities;

namespace Onibi_Pro.Application.Menus.Commands.CreateMenu;
internal sealed class CreateMenuCommandHandler : IRequestHandler<CreateMenuCommand, ErrorOr<Menu>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateMenuCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Menu>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var menu = Menu.Create(
            name: request.Name,
            menuItems: request.MenuItems.ConvertAll(menuItem => MenuItem.Create(
                name: menuItem.Name,
                price: menuItem.Price,
                ingredients: menuItem.Ingredients.ConvertAll(ingredient => Ingredient.Create(
                    name: ingredient.Name,
                    unitType: Enum.Parse<UnitType>(ingredient.Unit),
                    quantity: ingredient.Quantity)))));

        if (menu.IsError)
        {
            return menu.Errors;
        }

        await _unitOfWork.MenuRepository.AddAsync(menu.Value, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        return menu;
    }
}
