using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;

namespace Onibi_Pro.Application.RegionalManagers.Commands.UpdateRegionalManager;
internal sealed class UpdateRegionalManagerCommandHandler : IRequestHandler<UpdateRegionalManagerCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;
    private readonly IRestaurantDetailsService _restaurantDetailsService;

    public UpdateRegionalManagerCommandHandler(IUnitOfWork unitOfWork,
        IRegionalManagerDetailsService regionalManagerDetailsService,
        IRestaurantDetailsService restaurantDetailsService)
    {
        _unitOfWork = unitOfWork;
        _regionalManagerDetailsService = regionalManagerDetailsService;
        _restaurantDetailsService = restaurantDetailsService;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateRegionalManagerCommand request, CancellationToken cancellationToken)
    {
        var regionalManager = await _unitOfWork.RegionalManagerRepository.GetByIdAsync(request.RegionalManagerId, cancellationToken);

        if (regionalManager is null)
        {
            return Errors.RegionalManager.RegionalManagerNotFound;
        }

        var regionalManagerUserId = await _regionalManagerDetailsService.GetUserId(request.RegionalManagerId);
        var user = await _unitOfWork.UserRepository.GetByIdAsync(regionalManagerUserId, cancellationToken);

        if (user is null)
        {
            return Errors.RegionalManager.RegionalManagerNotFound;
        }

        user.Update(request.FirstName, request.LastName, request.Email);
        // Only new ones, removing will require some domain logic TODO
        var newRestaurantIds = (request.RestaurantIds ?? []).Except(regionalManager.Restaurants).ToList();
        var areRestaurantsAlreadyAssigned = await _restaurantDetailsService.AreRestaurantsAssignedToAnyRegionalManager(newRestaurantIds);

        foreach (var restaurant in newRestaurantIds)
        {
            var result = regionalManager.AssignRestaurant(restaurant, areRestaurantsAlreadyAssigned);

            if (result.IsError)
            {
                return result.Errors;
            }
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return new Success();
    }
}
