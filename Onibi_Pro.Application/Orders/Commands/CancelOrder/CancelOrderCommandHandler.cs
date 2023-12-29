using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Application.Services.Access;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Orders.Commands.CancelOrder;
internal sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAccessService _accessService;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IManagerDetailsService managerDetailsService,
        ICurrentUserService currentUserService,
        IAccessService accessService,
        IDbConnectionFactory dbConnectionFactory)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _managerDetailsService = managerDetailsService;
        _currentUserService = currentUserService;
        _accessService = accessService;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<ErrorOr<Success>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            return Errors.Order.OrderNotFound;
        }

        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));

        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);
        var canCancel = await _accessService.CanManagerCancelOrder(managerDetails.ManagerId, request.OrderId.Value, connection);

        if (!canCancel)
        {
            return Errors.Order.OperationForbidden;
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
