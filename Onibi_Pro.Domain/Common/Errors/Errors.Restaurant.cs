using ErrorOr;

namespace Onibi_Pro.Domain.Common.Errors;
public static partial class Errors
{
    public static class Restaurant
    {
        public static Error InvalidOrderId => Error.Validation(
            code: "Restaurant.InvalidOrderId", description: "Order Id is invalid.");
        
        public static Error RestaurantNotFound => Error.Validation(
            code: "Restaurant.NotFound", description: "Restaurant not found.");

        public static Error InvalidManager => Error.Validation(
            code: "Restaurant.InvalidManager", description: "This manager cannot perform this operation.");

        public static Error DuplicatedEmail => Error.Conflict(
            code: "Employee.DuplicatedEmail", description: "Duplicated email.");

        public static Error EmployeeNotFound => Error.NotFound(
            code: "Employee.NotFound", description: "Employee does not exist.");
    }
}
