using ErrorOr;

using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Commands.PickUpPackage;
internal sealed class PickUpPackageCommandHandler : IRequestHandler<PickUpPackageCommand, ErrorOr<Success>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ICourierDetailsService _courierDetailsService;
    private readonly IUnitOfWork _unitOfWork;

    public PickUpPackageCommandHandler(ICurrentUserService currentUserService,
        ICourierDetailsService courierDetailsService,
        IUnitOfWork unitOfWork)
    {
        _currentUserService = currentUserService;
        _courierDetailsService = courierDetailsService;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Success>> Handle(PickUpPackageCommand request, CancellationToken cancellationToken)
    {
        var package = await _unitOfWork.PackageRepository.GetByIdAsync(request.PackageId, cancellationToken);

        if (package is null)
        {
            return Errors.Package.PackageNotFound;
        }

        var courierDetails = await _courierDetailsService.GetCourierDetailsAsync(UserId.Create(_currentUserService.UserId));

        var result = package.PickUp(CourierId.Create(courierDetails.CourierId));

        if (result.IsError)
        {
            return result.Errors;
        }

        await _unitOfWork.SaveAsync(cancellationToken);

        return new Success();
    }
}
