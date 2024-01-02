namespace Onibi_Pro.Contracts.RegionalManagers;
public record GetCouriersResponse(Guid CourierId, string Phone, string FirstName, string LastName, string Email);