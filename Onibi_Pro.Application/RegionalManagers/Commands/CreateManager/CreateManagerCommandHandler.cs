using ErrorOr;

using MediatR;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Application.Services.CuttingConcerns;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;
using Onibi_Pro.Shared;

namespace Onibi_Pro.Application.RegionalManagers.Commands.CreateManager;
internal sealed class CreateManagerCommandHandler : IRequestHandler<CreateManagerCommand, ErrorOr<Success>>
{
    private readonly IAssignManagerService _assignManagerService;
    private readonly IRegisterService _registerService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateManagerCommandHandler> _logger;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;

    public CreateManagerCommandHandler(IAssignManagerService assignManagerService,
        IRegisterService registerService,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        ILogger<CreateManagerCommandHandler> logger,
        IRegionalManagerDetailsService regionalManagerDetailsService)
    {
        _assignManagerService = assignManagerService;
        _registerService = registerService;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _regionalManagerDetailsService = regionalManagerDetailsService;
    }
    public async Task<ErrorOr<Success>> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var user = await _registerService.RegisterAsync(request.FirstName, request.LastName,
            request.Email, Passwords.InitialPassword, CreatorUserType.Create(_currentUserService.UserType), cancellationToken, commitTransaction: false);

        if (user.IsError)
        {
            _logger.LogError("An error occured when creating a user.");
            if (user.FirstError != Error.Unexpected())
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            }

            return user.Errors;
        }

        var regionalManagerDetails =
            await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));

        if (!regionalManagerDetails.RestaurantIds.Any(id => id == request.RestaurantId.Value))
        {
            _logger.LogError("User {currentUserId} is attempting to assign user {createdUserId} " +
                "as the new manager for restaurant {restaurantId}, even though the restaurant does not belong to them",
                _currentUserService.UserId, user.Value.Value, request.RestaurantId.Value);
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            return Errors.Restaurant.RestaurantNotFound;
        }

        var result = await _assignManagerService.AssignToRestaurant(request.RestaurantId, user.Value);

        if (result.IsError)
        {
            _logger.LogError("An error occured when assigning to restaurant.");
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            return result.Errors;
        }

        try
        {
            await _unitOfWork.SaveAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user and assign as manager of restaurant {restaurantId}", request.RestaurantId.Value);
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);

            return Error.Unexpected();
        }

        return new Success();
    }
}
