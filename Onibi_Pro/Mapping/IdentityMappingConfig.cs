using Mapster;

using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Application.Identity.Queries.GetWhoami;
using Onibi_Pro.Contracts.Identity;

namespace Onibi_Pro.Mapping;

public sealed class IdentityMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<WhoamiDto, GetWhoamiResponse>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.UserId, src => src.UserId.Value)
            .Map(dest => dest.UserType, src => src.UserType.ToString());

        config.NewConfig<ManagerDetailsDto, GetManagerDetailsResponse>()
            .Map(dest => dest, src => src);
    }
}
