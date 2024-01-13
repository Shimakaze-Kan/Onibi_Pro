using MediatR;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.Events;

namespace Onibi_Pro.Application.Restaurants.Events;
internal sealed class AssignRestaurantToRegionalManager : INotificationHandler<RestaurantCreated>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AssignRestaurantToRegionalManager> _logger;
    private readonly IRestaurantDetailsService _restaurantDetailsService;

    public AssignRestaurantToRegionalManager(IUnitOfWork unitOfWork,
        ILogger<AssignRestaurantToRegionalManager> logger,
        IRestaurantDetailsService restaurantDetailsService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _restaurantDetailsService = restaurantDetailsService;
    }

    public async Task Handle(RestaurantCreated notification, CancellationToken cancellationToken)
    {
        var regionalManager = await _unitOfWork.RegionalManagerRepository.GetByIdAsync(notification.RegionalManagerId, cancellationToken);

        if (regionalManager is null)
        {
            _logger.LogCritical("Regional Manager Id: {regionalManagerId} not found.", notification.RegionalManagerId.Value);
            throw new ArgumentNullException(Errors.RegionalManager.RegionalManagerNotFound.Description);
        }

        var isRestaurantsAssignedToAnyRegionalManager = 
            await _restaurantDetailsService.AreRestaurantsAssignedToAnyRegionalManager([notification.Restaurant.Id]);
        var result = regionalManager.AssignRestaurant(notification.Restaurant.Id, isRestaurantsAssignedToAnyRegionalManager);

        if (result.IsError)
        {
            throw new ArgumentException(result.FirstError.Description);
        }

        await _unitOfWork.SaveAsync(cancellationToken);
        _logger.LogInformation("Restaurant: {restaurantId}, asigned to Regional Manager: {regionalManagerId}",
            notification.Restaurant.Id.Value, notification.RegionalManagerId.Value);
    }
}
