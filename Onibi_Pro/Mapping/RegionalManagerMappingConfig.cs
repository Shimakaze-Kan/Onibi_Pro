using Mapster;

using Onibi_Pro.Application.RegionalManagers.Commands.CreateCourier;
using Onibi_Pro.Contracts.RegionalManagers;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Mapping;

public class RegionalManagerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCourierRequest, CreateCourierCommand>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.UserId, src => UserId.Create(src.UserId));
    }
}
