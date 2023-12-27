using Mapster;

using Onibi_Pro.Application.Menus.Commands.CreateMenu;
using Onibi_Pro.Application.Menus.Queries.GetIngredients;
using Onibi_Pro.Application.Menus.Queries.GetMenus;
using Onibi_Pro.Contracts.Menus;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.MenuAggregate;
using Onibi_Pro.Domain.MenuAggregate.Entities;

namespace Onibi_Pro.Mapping;

public class MenuMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateMenuRequest, CreateMenuCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<Menu, CreateMenuResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        
        config.NewConfig<MenuItem, MenuItemResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<Ingredient, IngredientResponse>()
            .Map(dest => dest.Unit, src => Enum.GetName(src.Unit));

        config.NewConfig<IngredientDto, GetIngredientResponse>()
            .Map(dest => dest.Unit, src => Enum.GetName(src.Unit));

        config.NewConfig<IngredientKeyValueDto, GetIngredientsResponse>()
            .Map(dest => dest.Unit, src => Enum.GetName(src.Unit));
    }
}
