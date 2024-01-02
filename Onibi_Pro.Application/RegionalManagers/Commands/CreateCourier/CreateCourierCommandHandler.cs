using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RegionalManagerAggregate.Entities;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.RegionalManagers.Commands.CreateCourier;
internal sealed class CreateCourierCommandHandler : IRequestHandler<CreateCourierCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;

    public CreateCourierCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IRegionalManagerDetailsService regionalManagerDetailsService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
    }

    public async Task<ErrorOr<Success>> Handle(CreateCourierCommand request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Errors.User.UserNotFound;
        }

        var regionalmanagerDetails = await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var regionalManagerId = RegionalManagerId.Create(regionalmanagerDetails.RegionalManagerId);
        var regionalManager = await _unitOfWork.RegionalManagerRepository.GetByIdAsync(
            regionalManagerId, cancellationToken);

        if (regionalManager is null)
        {
            return Errors.RegionalManager.RegionalManagerNotFound;
        }

        var courier = Courier.CreateUnique(request.UserId, request.Phone, user.UserType);

        if (courier.IsError)
        {
            return courier.Errors;
        }

        var result = regionalManager.AssignCourier(courier.Value, regionalManagerId);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return new Success();
    }
}
