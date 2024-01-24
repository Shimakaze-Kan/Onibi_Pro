using Mapster;

using Onibi_Pro.Application.Authentication.Commands;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Contracts.Authentication;
using Onibi_Pro.Domain.UserAggregate;

namespace Onibi_Pro.Mapping;

public sealed class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticationResult, AuthenticationResponse>();

        config.NewConfig<RegisterRequest, RegisterCommand>()
            .Map(dest => dest.UserType, src => Enum.Parse<UserTypes>(src.UserType));
    }
}
