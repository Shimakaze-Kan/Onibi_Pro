using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.Events;

namespace Onibi_Pro.Application.Authentication.Events;
internal sealed class PopulateUserToMasterDb : INotificationHandler<UserCreated>
{
    private readonly IMasterDbRepository _masterDbRepository;
    private readonly ICurrentUserService _currentUserService;

    public PopulateUserToMasterDb(IMasterDbRepository masterDbRepository,
        ICurrentUserService currentUserService)
    {
        _masterDbRepository = masterDbRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _masterDbRepository.AddUser(notification.Email, _currentUserService.ClientName);
    }
}
