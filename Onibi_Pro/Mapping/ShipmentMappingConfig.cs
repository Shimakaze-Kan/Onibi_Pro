using Mapster;

using Onibi_Pro.Application.Packages.Commands.CreatePackage;
using Onibi_Pro.Contracts.Packages;
using Onibi_Pro.Domain.Common.ValueObjects;

namespace Onibi_Pro.Mapping;

public class ShipmentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreatePackageRequest, CreatePackageCommand>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.Ingredients, src => src.Ingredients);

        TypeAdapterConfig<CreatePackageRequest.Ingredient, Ingredient>.NewConfig()
            .ConstructUsing(src => Ingredient.Create(src.Name, Enum.Parse<UnitType>(src.Unit), src.Quantity));
    }
}
