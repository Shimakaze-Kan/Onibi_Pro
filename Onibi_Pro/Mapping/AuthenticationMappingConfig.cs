using Mapster;
using Onibi_Pro.Application.Services.Authentication;
using Onibi_Pro.Contracts.Authentication;

namespace Onibi_Pro.Mapping;

public sealed class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthenticationResult, AuthenticationResponse>();
    }
}
