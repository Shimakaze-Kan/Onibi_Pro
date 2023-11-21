using Onibi_Pro.Application.Common.Interfaces.Services;

namespace Onibi_Pro.Infrastructure.Services;
internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
