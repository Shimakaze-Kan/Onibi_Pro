using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;

namespace Onibi_Pro.Application.Menus.Commands.RemoveMenuItem;
internal sealed class RemoveMenuItemCommandHandler : IRequestHandler<RemoveMenuItemCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveMenuItemCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Success>> Handle(RemoveMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menu = await _unitOfWork.MenuRepository.GetByIdAsync(request.MenuId, cancellationToken);

        if (menu is null)
        {
            return Errors.Menu.MenuNotFound;
        }

        var menuItem = menu.MenuItems.FirstOrDefault(item => item.Id == request.MenuItemId);

        if (menuItem is null)
        {
            return Errors.Menu.MenuItemNotFound;
        }

        menu.RemoveItem(menuItem);

        await _unitOfWork.SaveAsync(cancellationToken);

        return new Success();
    }
}
