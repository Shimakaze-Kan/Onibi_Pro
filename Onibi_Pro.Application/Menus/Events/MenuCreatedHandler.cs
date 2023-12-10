using MediatR;

using Onibi_Pro.Domain.MenuAggregate.Events;

namespace Onibi_Pro.Application.Menus.Events;
internal sealed class MenuCreatedHandler : INotificationHandler<MenuCreated>
{
    public Task Handle(MenuCreated notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
