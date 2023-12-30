using Mapster;

using Onibi_Pro.Application.Packages.Commands.CreatePackage;
using Onibi_Pro.Application.Packages.Queries.Common;
using Onibi_Pro.Contracts.Shipments;
using Onibi_Pro.Contracts.Shipments.Common;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.PackageAggregate;

using Ingredient = Onibi_Pro.Domain.Common.ValueObjects.Ingredient;

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

        TypeAdapterConfig<Ingredient, PackageItem.Ingredient>.NewConfig()
            .ConstructUsing(src => new PackageItem.Ingredient(src.Name, Enum.GetName(src.Unit)!, src.Quantity));

        config.NewConfig<Package, PackageItem>()
            .Map(dest => dest.Manager, src => src.Manager.Value)
            .Map(dest => dest.RegionalManager, src => src.RegionalManager.Value)
            .Map(dest => dest.SourceRestaurant, src => src.SourceRestaurant.Value)
            .Map(dest => dest.Courier, src => src.Courier.Value)
            .Map(dest => dest.OriginAddress, src => src.Origin)
            .Map(dest => dest.DestinationAddress, src => src.Destination)
            .Map(dest => dest.DestinationRestaurant, src => src.DestinationRestaurant.Value)
            .Map(dest => dest.Status, src => Enum.GetName(src.Status))
            .Map(dest => dest.Message, src => src.Message)
            .Map(dest => dest.IsUrgent, src => src.IsUrgent)
            .Map(dest => dest.Until, src => src.Until)
            .Map(dest => dest.Ingredients, src => src.Ingredients);

        config.NewConfig<PackageDto, PackageItem>()
            .Map(dest => dest.Status, src => Enum.GetName(src.Status))
            .Map(dest => dest.Ingredients, src => src.IngredientsList)
            .Map(dest => dest.OriginAddress, src =>
                string.IsNullOrEmpty(src.OriginStreet) &&
                string.IsNullOrEmpty(src.OriginCity) &&
                string.IsNullOrEmpty(src.OriginPostalCode) &&
                string.IsNullOrEmpty(src.OriginCountry)
                    ? null
                    : new Contracts.Common.Address(
                        src.OriginStreet,
                        src.OriginCity,
                        src.OriginPostalCode,
                        src.OriginCountry))
            .Map(dest => dest.DestinationAddress, src
                => new Contracts.Common.Address(
                    src.DestinationStreet,
                    src.DestinationCity,
                    src.DestinationPostalCode,
                    src.DestinationCountry));
    }
}
