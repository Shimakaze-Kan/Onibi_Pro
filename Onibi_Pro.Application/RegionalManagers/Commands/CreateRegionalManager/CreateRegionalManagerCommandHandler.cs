using ErrorOr;

using MediatR;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Domain.RegionalManagerAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Commands.CreateRegionalManager;
internal sealed class CreateRegionalManagerCommandHandler : IRequestHandler<CreateRegionalManagerCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRestaurantDetailsService _restaurantDetailsService;
    private readonly ILogger<CreateRegionalManagerCommandHandler> _logger;
    private readonly IRegisterService _registerService;
    private readonly ICurrentUserService _currentUserService;

    public CreateRegionalManagerCommandHandler(IUnitOfWork unitOfWork,
        IRestaurantDetailsService restaurantDetailsService,
        ILogger<CreateRegionalManagerCommandHandler> logger,
        IRegisterService registerService,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _restaurantDetailsService = restaurantDetailsService;
        _logger = logger;
        _registerService = registerService;
        _currentUserService = currentUserService;
    }

    public async Task<ErrorOr<Success>> Handle(CreateRegionalManagerCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var registerRestult = await _registerService.RegisterAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            "pass123@!",
            CreatorUserType.Create(_currentUserService.UserType),
            cancellationToken,
            commitTransaction: false);

        if (registerRestult.IsError)
        {
            _logger.LogError("An error occured when creating a user.");

            if (registerRestult.FirstError != Error.Unexpected())
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            }

            return registerRestult.Errors;
        }

        var areRestaurantsAssignedToAnyRegionalManager =
            await _restaurantDetailsService.AreRestaurantsAssignedToAnyRegionalManager(request.RestaurantIds);

        var regionalManager = RegionalManager.Create(userId: registerRestult.Value, areRestaurantsAssignedToAnyRegionalManager, request.RestaurantIds);

        if (regionalManager.IsError)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError("An error occured when creating regional manager.");

            return regionalManager.Errors;
        }

        await _unitOfWork.RegionalManagerRepository.AddAsync(regionalManager.Value, cancellationToken);

        try
        {
            await _unitOfWork.SaveAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Regional Manager");
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            return Error.Unexpected();
        }

        return new Success();
    }
}
