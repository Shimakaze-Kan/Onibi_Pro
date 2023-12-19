using Onibi_Pro.Application.Common.Models;
using Onibi_Pro.Domain.UserAggregate.ValueObjects;

namespace Onibi_Pro.Application.Common.Interfaces.Services;
public interface IManagerDetailsService
{
    Task<ManagerDetailsDto> GetManagerDetailsAsync(UserId userId);
}
