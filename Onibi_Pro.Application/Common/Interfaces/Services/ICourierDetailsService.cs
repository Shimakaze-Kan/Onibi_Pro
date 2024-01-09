using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.RegionalManagerAggregate.ValueObjects;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Common.Interfaces.Services;
public interface ICourierDetailsService
{
    Task<CourierDetailsDto> GetCourierDetailsAsync(UserId userId);
    Task<UserId> GetUserId(CourierId courierId);
}
