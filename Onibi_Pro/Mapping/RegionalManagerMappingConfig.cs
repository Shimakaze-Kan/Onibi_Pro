using Mapster;

using Onibi_Pro.Application.RegionalManagers.Commands.CreateCourier;
using Onibi_Pro.Application.RegionalManagers.Commands.CreateManager;
using Onibi_Pro.Application.RegionalManagers.Commands.UpdateManager;
using Onibi_Pro.Contracts.RegionalManagers;
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
    }
}
