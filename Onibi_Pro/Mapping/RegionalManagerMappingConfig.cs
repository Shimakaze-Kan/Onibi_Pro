using Mapster;

using Onibi_Pro.Application.RegionalManagers.Commands.CreateCourier;
using Onibi_Pro.Application.RegionalManagers.Commands.CreateManager;
using Onibi_Pro.Application.RegionalManagers.Commands.UpdateManager;
using Onibi_Pro.Application.RegionalManagers.Commands.UpdateRegionalManager;
using Onibi_Pro.Contracts.RegionalManagers;
using Onibi_Pro.Domain.PackageAggregate;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Mapping;

public class RegionalManagerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCourierRequest, CreateCourierCommand>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.UserId, src => UserId.Create(src.UserId));

        config.NewConfig<CreateManagerRequest, CreateManagerCommand>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.RestaurantId, src => RestaurantId.Create(src.RestaurantId));

        config.NewConfig<UpdateManagerRequest, UpdateManagerCommand>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.RestaurantId, src => RestaurantId.Create(src.RestaurantId))
            .Map(dest => dest.ManagerId, src => ManagerId.Create(src.ManagerId));

        TypeAdapterConfig<Guid, RestaurantId>.NewConfig()
            .ConstructUsing(src => RestaurantId.Create(src));

        config.NewConfig<CreateRegionalManagerRequest, CreateRegionalManagerRequest>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.RestaurantIds, src => src.RestaurantIds.ConvertAll(id => RestaurantId.Create(id)), x => x.RestaurantIds != null);

        config.NewConfig<UpdateRegionalManagerRequest, UpdateRegionalManagerCommand>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.RegionalManagerId, src => RegionalManagerId.Create(src.RegionalManagerId))
            .Map(dest => dest.RestaurantIds, src => src.RestaurantIds.ConvertAll(id => RestaurantId.Create(id)), x => x.RestaurantIds != null);
    }
}
