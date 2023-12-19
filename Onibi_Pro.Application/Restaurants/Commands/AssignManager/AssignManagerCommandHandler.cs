using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Restaurants.Commands.AssignManager;
internal sealed class AssignManagerCommandHandler : IRequestHandler<AssignManagerCommand, ErrorOr<Unit>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AssignManagerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Unit>> Handle(AssignManagerCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _unitOfWork.RestaurantRepository
            .GetByIdAsync(RestaurantId.Create(request.RestaurantId), cancellationToken);

        if (restaurant is null)
        {
            return Errors.Restaurant.RestaurantNotFound;
        }

        var user = await _unitOfWork.UserRepository.GetByIdAsync(UserId.Create(request.UserId), cancellationToken);

        if (user is null)
        {
            return Errors.User.UserNotFound;
        }

        var result = restaurant.AssigneManager(user.Id, user.UserType);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}
