using ErrorOr;

using MediatR;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Orders.Commands.CancelOrder;
internal sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CancelOrderCommandHandler> _logger;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IManagerDetailsService managerDetailsService,
        ICurrentUserService currentUserService,
        ILogger<CancelOrderCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _managerDetailsService = managerDetailsService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Errors.Order.OrderNotFound;
        }

        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(
            RestaurantId.Create(managerDetails.RestaurantId), cancellationToken);

        if (restaurant is null)
        {
            _logger.LogCritical("Restaurant: {restaurantId} not found. User Id: {userId}, Manager Id: {managerId}.",
                managerDetails.RestaurantId, _currentUserService.UserId, managerDetails.ManagerId);

            return Error.Unexpected();
        }

        if (!restaurant.DoesOrderExist(request.OrderId))
        {
            return Errors.Order.OrderNotFound;
        }

        var result = order.Cancel(_dateTimeProvider.UtcNow);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        return new Success();
    }
}
