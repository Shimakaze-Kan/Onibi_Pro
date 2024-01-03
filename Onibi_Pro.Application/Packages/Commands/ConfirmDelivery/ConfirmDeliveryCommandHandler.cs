using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Commands.ConfirmDelivery;
internal sealed class ConfirmDeliveryCommandHandler : IRequestHandler<ConfirmDeliveryCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IManagerDetailsService _managerDetailsService;

    public ConfirmDeliveryCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IManagerDetailsService managerDetailsService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _managerDetailsService = managerDetailsService;
    }

    public async Task<ErrorOr<Success>> Handle(ConfirmDeliveryCommand request, CancellationToken cancellationToken)
    {
        var package = await _unitOfWork.PackageRepository.GetByIdAsync(request.PackageId, cancellationToken);

        if (package is null)
        {
            return Errors.Package.PackageNotFound;
        }

        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var managersRestaurantId = RestaurantId.Create(managerDetails.RestaurantId);

        var result = package.ConfirmDelivery(managersRestaurantId);

        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return new Success();
    }
}
