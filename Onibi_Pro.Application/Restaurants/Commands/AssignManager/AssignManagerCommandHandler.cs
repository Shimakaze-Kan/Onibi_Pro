using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Services.CuttingConcerns;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.AssignManager;
internal sealed class AssignManagerCommandHandler : IRequestHandler<AssignManagerCommand, ErrorOr<Unit>>
{
    private readonly IAssignManagerService _assignManagerService;

    public AssignManagerCommandHandler(IAssignManagerService assignManagerService)
    {
        _assignManagerService = assignManagerService;
    }

    public async Task<ErrorOr<Unit>> Handle(AssignManagerCommand request, CancellationToken cancellationToken)
    {
        var restaurantId = RestaurantId.Create(request.RestaurantId);
        var userId = UserId.Create(request.UserId);

        var result = await _assignManagerService.AssignToRestaurant(restaurantId, userId, cancellationToken);

        if (result.IsError)
        {
            return result.Errors;
        }

        return Unit.Value;
    }
}
