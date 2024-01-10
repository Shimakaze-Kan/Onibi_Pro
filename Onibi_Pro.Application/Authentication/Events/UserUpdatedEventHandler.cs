using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.Events;

namespace Onibi_Pro.Application.Authentication.Events;
internal sealed class UserUpdatedEventHandler : INotificationHandler<UserUpdated>
{
    private readonly IMasterDbRepository _masterDbRepository;
    private readonly ICurrentUserService _currentUserService;

    public UserUpdatedEventHandler(IMasterDbRepository masterDbRepository,
        ICurrentUserService currentUserService)
    {
        _masterDbRepository = masterDbRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UserUpdated notification, CancellationToken cancellationToken)
    {
        await _masterDbRepository.UpdateUser(notification.OldEmail, notification.NewEmail, _currentUserService.ClientName);
    }
}
