using MediatR;

using Onibi_Pro.Application.Common.Interfaces.Services;
using Onibi_Pro.Application.Persistence;
using Onibi_Pro.Domain.PackageAggregate;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Packages.Commands.CreatePackage;
internal sealed class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, Package>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IManagerDetailsService _managerDetailsService;
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public CreatePackageCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IManagerDetailsService managerDetailsService,
        IDbConnectionFactory dbConnectionFactory)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _managerDetailsService = managerDetailsService;
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Package> Handle(CreatePackageCommand request, CancellationToken cancellationToken)
    {
        using var connection = await _dbConnectionFactory.OpenConnectionAsync(_currentUserService.ClientName);
        var managerDetails = await _managerDetailsService.GetManagerDetailsAsync(UserId.Create(_currentUserService.UserId));
        var restaurant = await _unitOfWork.RestaurantRepository.GetByIdAsync(RestaurantId.Create(managerDetails.RestaurantId), cancellationToken);

        var package = Package.Create(
            ManagerId.Create(managerDetails.ManagerId),
            RegionalManagerId.Create(managerDetails.RegionalManagerId),
            restaurant!.Address,
            restaurant.Id,
            request.Message,
            [.. request.Ingredients],
            request.Until,
            request.IsUrgent);

        await _unitOfWork.PackageRepository.AddAsync(package, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return package;
    }
}
