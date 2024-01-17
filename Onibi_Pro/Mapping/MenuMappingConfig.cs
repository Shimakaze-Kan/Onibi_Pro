using Mapster;

using Onibi_Pro.Application.Menus.Commands.AddMenuItem;
using Onibi_Pro.Application.Menus.Commands.CreateMenu;
using Onibi_Pro.Application.Menus.Commands.RemoveMenuItem;
using Onibi_Pro.Application.Menus.Queries.GetIngredients;
using Onibi_Pro.Application.Menus.Queries.GetMenus;
using Onibi_Pro.Contracts.Menus;
using Onibi_Pro.Domain.Common.ValueObjects;
using Onibi_Pro.Domain.MenuAggregate;
using Onibi_Pro.Domain.MenuAggregate.Entities;
using Onibi_Pro.Domain.MenuAggregate.ValueObjects;

namespace Onibi_Pro.Mapping;

public class MenuMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateMenuRequest, CreateMenuCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<Menu, CreateMenuResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        
        config.NewConfig<MenuItem, CreateMenuResponse.MenuItem>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<Ingredient, CreateMenuResponse.Ingredient>()
            .Map(dest => dest.Unit, src => Enum.GetName(src.Unit));

        config.NewConfig<IngredientDto, GetMenusResponse.Ingredient>()
            .Map(dest => dest.Unit, src => Enum.GetName(src.Unit));

        config.NewConfig<IngredientKeyValueDto, GetIngredientResponse>()
            .Map(dest => dest.Unit, src => Enum.GetName(src.Unit));

        config.NewConfig<RemoveMenuItemRequest, RemoveMenuItemCommand>()
            .Map(dest => dest.MenuId, src => MenuId.Create(src.MenuId))
            .Map(dest => dest.MenuItemId, src => MenuItemId.Create(src.MenuItemId));

        config.NewConfig<AddMenuItemRequest, AddMenuItemCommand>()
            .Map(dest => dest.MenuId, src => MenuId.Create(src.MenuId));

        TypeAdapterConfig<AddMenuItemRequest.Ingredient, Ingredient>.NewConfig()
            .ConstructUsing(src => Ingredient.Create(src.Name, Enum.Parse<UnitType>(src.Unit), src.Quantity));
    }
}
