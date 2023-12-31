using Mapster;

using Onibi_Pro.Application.Restaurants.Commands.AssignManager;
using Onibi_Pro.Application.Restaurants.Commands.CreateEmployee;
using Onibi_Pro.Application.Restaurants.Commands.CreateRestaurant;
using Onibi_Pro.Application.Restaurants.Commands.CreateSchedule;
using Onibi_Pro.Application.Restaurants.Commands.DeleteSchedule;
using Onibi_Pro.Application.Restaurants.Commands.EditEmployee;
using Onibi_Pro.Application.Restaurants.Commands.EditSchedule;
using Onibi_Pro.Application.Restaurants.Queries.GetEmployees;
using Onibi_Pro.Contracts.Restaurants;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.RestaurantAggregate;
using Onibi_Pro.Domain.RestaurantAggregate.Entities;

namespace Onibi_Pro.Mapping;

public class RestaurantMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateRestaurantRequest, CreateRestaurantCommand>()
            .Map(dest => dest, src => src)
            .Map(dest => dest.RegionalManagerId, src => RegionalManagerId.Create(src.RegionalManagerId));

        config.NewConfig<Restaurant, CreateRestaurantResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<Employee, CreateRestaurantResponse.Employee>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.EmployeePositions, src =>
                src.Positions.Select(position =>
                    new CreateRestaurantResponse.EmployeePosition(Enum.GetName(position.Position)!)).ToList());

        config.NewConfig<Manager, CreateRestaurantResponse.Manager>()
            .Map(dest => dest.Id, src => src.Id.Value);

        config.NewConfig<(Guid RestaurantId, GetEmployeeRequest Request), GetEmployeesQuery>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.RestaurantId, src => src.RestaurantId)
            .Map(dest => dest.PositionFilter, src =>
                src.Request.PositionFilterList
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .ToList());

        config.NewConfig<(Guid RestaurantId, CreateEmployeeRequest Request), CreateEmployeeCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.RestaurantId, src => src.RestaurantId);
        
        config.NewConfig<(Guid RestaurantId, EditEmployeeRequest Request), EditEmployeeCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.RestaurantId, src => src.RestaurantId);

        config.NewConfig<Employee, CreateEmployeeResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.EmployeePositions, src =>
                src.Positions.Select(position =>
                    Enum.GetName(position.Position)!).ToList());

        config.NewConfig<(Guid RestaurantId, AssignManagerRequest Request), AssignManagerCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.RestaurantId, src => src.RestaurantId);

        config.NewConfig<(Guid RestaurantId, CreateScheduleRequest Request), CreateScheduleCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.RestaurantId, src => src.RestaurantId);
        
        config.NewConfig<(Guid RestaurantId, EditScheduleRequest Request), EditScheduleCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.RestaurantId, src => src.RestaurantId);

        config.NewConfig<(Guid RestaurantId, DeleteScheduleRequest Request), DeleteScheduleCommand>()
            .Map(dest => dest, src => src.Request)
            .Map(dest => dest.RestaurantId, src => src.RestaurantId);
    }
}
