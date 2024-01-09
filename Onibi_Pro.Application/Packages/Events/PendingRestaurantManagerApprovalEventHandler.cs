using MediatR;

using Onibi_Pro.Domain.PackageAggregate.Events;

namespace Onibi_Pro.Application.Packages.Events;
internal sealed class PendingRestaurantManagerApprovalEventHandler : INotificationHandler<PendingRestaurantManagerApproval>
{
    public Task Handle(PendingRestaurantManagerApproval notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException(); // TODO
    }


}
