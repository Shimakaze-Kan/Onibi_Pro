using ErrorOr;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.Services;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;


namespace Onibi_Pro.Application.Services.CuttingConcerns;
internal class AssignManagerService : IAssignManagerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantDomainService _restaurantDomainService;
    private readonly IManagerDetailsService _managerDetailsService;

    public AssignManagerService(IUnitOfWork unitOfWork,
        IRestaurantDomainService restaurantDomainService,
        IManagerDetailsService managerDetailsService)
    {
        _unitOfWork = unitOfWork;
        _restaurantDomainService = restaurantDomainService;
        _managerDetailsService = managerDetailsService;
    }

    public async Task<ErrorOr<Success>> AssignToRestaurant(RestaurantId restaurantId,
        UserId userId, CancellationToken cancellationToken = default)
    {
        var restaurantIdValue = restaurantId.Value;
        var userIdValue = userId.Value;

        var destinationRestaurant = await _unitOfWork.RestaurantRepository
            .GetByIdAsync(RestaurantId.Create(restaurantIdValue), cancellationToken);

        if (destinationRestaurant is null)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var user = await _unitOfWork.UserRepository.GetByIdAsync(UserId.Create(userIdValue), cancellationToken);

        if (user is null)
        {
            return Errors.User.UserNotFound;
        }

        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(userId);

        Manager? manager = null;
        var sourceRestaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(
            RestaurantId.Create(managerDetails.RestaurantId), cancellationToken);

        if (managerDetails.RestaurantId != Guid.Empty)
        {
            manager = sourceRestaurant?.TryGetManager(userId);
        }

        manager ??= Manager.Create(userId);

        var result = _restaurantDomainService.AssignManagerToRestaurant(manager, destinationRestaurant, sourceRestaurant);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return new Success();
    }
}
