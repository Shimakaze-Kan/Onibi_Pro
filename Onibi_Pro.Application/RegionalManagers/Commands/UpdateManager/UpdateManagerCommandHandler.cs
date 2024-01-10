using ErrorOr;

using MediatR;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Application.Services.CuttingConcerns;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Commands.UpdateManager;
internal sealed class UpdateManagerCommandHandler : IRequestHandler<UpdateManagerCommand, ErrorOr<Success>>
{
    private readonly IAssignManagerService _assignManagerService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<UpdateManagerCommandHandler> _logger;

    public UpdateManagerCommandHandler(IAssignManagerService assignManagerService,
        IUnitOfWork unitOfWork,
        IManagerDetailsService managerDetailsService,
        IRegionalManagerDetailsService regionalManagerDetailsService,
        ICurrentUserService currentUserService,
        ILogger<UpdateManagerCommandHandler> logger)
    {
        _assignManagerService = assignManagerService;
        _unitOfWork = unitOfWork;
        _managerDetailsService = managerDetailsService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateManagerCommand request, CancellationToken cancellationToken)
    {
        var userId = await _managerDetailsService.GetUserId(request.ManagerId);

        if (userId is null)
        {
            return Errors.Restaurant.ManagerNotFound;
        }

        var user = await _unitOfWork.UserRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return Errors.Restaurant.ManagerNotFound;
        }

        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(userId);
        var restaurantId = RestaurantId.Create(managerDetails.RestaurantId);
        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(restaurantId, cancellationToken);

        if (restaurant is null)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var manager = restaurant.TryGetManager(userId);

        if (manager is null)
        {
            return Errors.Restaurant.ManagerNotFound;
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            user.Update(request.FirstName, request.LastName, request.Email);

            await _unitOfWork.SaveAsync(cancellationToken);

            if (request.RestaurantId != restaurantId)
            {
                var result = await MoveManagerToRestaurant(userId, request.RestaurantId, cancellationToken);

                if (result.IsError)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return result.Errors;
                }
            }

            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Updating manager {managerId} failed", request.ManagerId.Value);
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            return Error.Unexpected();
        }

        return new Success();
    }

    private async Task<ErrorOr<Success>> MoveManagerToRestaurant(UserId userId, RestaurantId restaurantId, CancellationToken cancellationToken)
    {
        var regionalManagerDetails = await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));

        if (!regionalManagerDetails.RestaurantIds.Any(id => id == restaurantId.Value))
        {
            _logger.LogError("User {currentUserId} is attempting to assign user {createdUserId} " +
                "as the new manager for restaurant {restaurantId}, even though the restaurant does not belong to them",
                _currentUserService.UserId, userId.Value, restaurantId.Value);

            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            return Errors.Restaurant.RestaurantNotFound;
        }
        var newRestaurantId = restaurantId;
        var newRestaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(newRestaurantId);

        await _assignManagerService.AssignToRestaurant(newRestaurantId, userId, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return new Success();
    }
}
