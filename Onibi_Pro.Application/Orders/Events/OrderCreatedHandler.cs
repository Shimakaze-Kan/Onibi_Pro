using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.OrderAggregate.Events;

namespace Onibi_Pro.Application.Orders.Events;
internal sealed class OrderCreatedHandler : INotificationHandler<OrderCreated>
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderCreatedHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(OrderCreated notification, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(notification.RestaurantId, cancellationToken);

        if (restaurant is null)
        {
            return;
        }

        restaurant.AddOrders([notification.OrderId]);
        await _unitOfWork.RestaurantRepository.UpdateAsync(restaurant, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}
