using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.UserAggregate.Events;

namespace Onibi_Pro.Application.Authentication.Events;
internal sealed class UserCreatedHandler : INotificationHandler<UserCreated>
{
    private readonly IUserPasswordRepository _userPasswordRepository;
    private readonly ICurrentUserService _currentUserService;

    public UserCreatedHandler(IUserPasswordRepository userPasswordRepository,
        ICurrentUserService currentUserService)
    {
        _userPasswordRepository = userPasswordRepository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UserCreated notification, CancellationToken cancellationToken)
    {
        await _userPasswordRepository.CreatePasswordAsync(
            notification.UserId, notification.HashedPassword, _currentUserService.ClientName, cancellationToken);
    }
}
