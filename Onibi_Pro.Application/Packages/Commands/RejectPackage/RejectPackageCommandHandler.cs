using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Commands.RejectPackage;
internal sealed class RejectPackageCommandHandler : IRequestHandler<RejectPackageCommand, ErrorOr<Success>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;
    private readonly IUnitOfWork _unitOfWork;

    public RejectPackageCommandHandler(ICurrentUserService currentUserService,
        IManagerDetailsService managerDetailsService,
        IRegionalManagerDetailsService regionalManagerDetailsService,
        IUnitOfWork unitOfWork)
    {
        _currentUserService = currentUserService;
        _managerDetailsService = managerDetailsService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Success>> Handle(RejectPackageCommand request, CancellationToken cancellationToken)
    {
        var package = await _unitOfWork.PackageRepository.GetByIdAsync(request.PackageId, cancellationToken);

        if (package is null)
        {
            return Errors.Package.PackageNotFound;
        }

        ErrorOr<Success> result;
        if (_currentUserService.UserType == UserTypes.Manager)
        {
            var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
            result = package.RejectPackageBySourceRestaurantManager(RestaurantId.Create(managerDetails.RestaurantId));
        }
        else
        {
            var regionalManagerDetails = await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
            result = package.RejectShipmentByRegionalManager(RegionalManagerId.Create(regionalManagerDetails.RegionalManagerId));
        }

        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return new Success();
    }
}
