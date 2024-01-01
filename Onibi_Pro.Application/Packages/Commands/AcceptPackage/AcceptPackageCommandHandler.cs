using ErrorOr;

using MediatR;

using Microsoft.Extensions.Logging;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.Common.Errors;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Commands.AcceptPackage;
internal sealed class AcceptPackageCommandHandler : IRequestHandler<AcceptPackageCommand, ErrorOr<Success>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IRegionalManagerDetailsService _regionalManagerDetailsService;
    private readonly ILogger<AcceptPackageCommandHandler> _logger;

    public AcceptPackageCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IRegionalManagerDetailsService regionalManagerDetailsService,
        ILogger<AcceptPackageCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _regionalManagerDetailsService = regionalManagerDetailsService;
        _logger = logger;
    }

    public async Task<ErrorOr<Success>> Handle(AcceptPackageCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var package = await _unitOfWork.PackageRepository.GetByIdAsync(request.PackageId, cancellationToken);

        if (package is null)
        {
            return Errors.Package.PackageNotFound;
        }

        var regionalManagerDetails = await _regionalManagerDetailsService.GetRegionalManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var regionalManagerId = RegionalManagerId.Create(regionalManagerDetails.RegionalManagerId);
        var isOriginRestaurant = IsOriginRestaurant(request);

        ErrorOr<Success> result;
        if (isOriginRestaurant)
        {
            result = package.AcceptShipmentFromRestaurant(regionalManagerId, request.Origin, request.SourceRestaurantId!);
        }
        else
        {
            result = package.AcceptShipmentByRegionalManager(regionalManagerId, request.Origin);
        }

        if (result.IsError)
        {
            return result.Errors;
        }

        try
        {
            await _unitOfWork.SaveAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogCritical(ex, "Error while accepting package by regional manager.");

            return Error.Unexpected();
        }

        return new Success();
    }

    private static bool IsOriginRestaurant(AcceptPackageCommand request)
    {
        return request.SourceRestaurantId is not null;
    }
}
