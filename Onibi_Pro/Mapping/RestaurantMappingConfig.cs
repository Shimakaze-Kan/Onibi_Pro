using Mapster;

using Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
using Onibi_Pro.Application.Restaurants.Queries.GetEmployees;
using Onibi_Pro.Contracts.Restaurants;
using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;

namespace Onibi_Pro.Mapping;

public class RestaurantMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateRestaurantRequest, CreateRestaurantCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<EmployeePositionRequest, EmployeePositionCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<Restaurant, CreateRestaurantResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OrderIds, src => src.OrderIds.Select(orderId => orderId.Value).ToList());

        config.NewConfig<Employee, CreateRestaurantEmployeeResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.EmployeePositions, src =>
                src.Positions.Select(position =>
                    new CreateRestaurantEmployeePositionResponse(Enum.GetName(position.Position)!)).ToList());

        config.NewConfig<Manager, CreateRestaurantManagerResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}
