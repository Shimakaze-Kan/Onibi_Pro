using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.MenuAggregate.Entities;

namespace Onibi_Pro.Application.Menus.Commands.AddMenuItem;
internal sealed class AddMenuItemCommandHandler : IRequestHandler<AddMenuItemCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddMenuItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Success>> Handle(AddMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menu = await _unitOfWork.MenuRepository.GetByIdAsync(request.MenuId, cancellationToken);

        if (menu is null)
        {
            return Errors.Menu.MenuNotFound;
        }

        var menuItem = MenuItem.Create(request.Name, request.Price, request.Ingredients);

        menu.AddItem(menuItem);

        await _unitOfWork.SaveAsync(cancellationToken);
        return new Success();
    }
}
