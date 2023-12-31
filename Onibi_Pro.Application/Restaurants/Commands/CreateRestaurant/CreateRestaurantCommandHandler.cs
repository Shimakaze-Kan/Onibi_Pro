using ErrorOr;

using MediatR;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate;

namespace Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
internal sealed class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, ErrorOr<Restaurant>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateRestaurantCommandHandler> _logger;

    public CreateRestaurantCommandHandler(IUnitOfWork unitOfWork,
        ILogger<CreateRestaurantCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<Restaurant>> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var regionalManager = await _unitOfWork.RegionalManagerRepository.GetByIdAsync(request.RegionalManagerId, cancellationToken);

        if (regionalManager is null)
        {
            return Errors.RegionalManager.RegionalManagerNotFound;
        }

        var address = Address.Create(request.Address.Street, request.Address.City, request.Address.PostalCode, request.Address.Country);
        var restaurant = Restaurant.Create(address, request.RegionalManagerId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _unitOfWork.RestaurantRepository.AddAsync(restaurant, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogCritical(ex, "Error while creating restaurant");

            return Error.Unexpected();
        }

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return restaurant;
    }
}
