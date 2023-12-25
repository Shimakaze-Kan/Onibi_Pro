using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Onibi_Pro.Infrastructure.Persistence.Interceptors;

namespace Onibi_Pro.Infrastructure.Persistence;
internal sealed class DbContextFactory
{
    private readonly IConfiguration _configuration;
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
    private readonly Dictionary<string, OnibiProDbContext> _contexts = [];

    public DbContextFactory(IConfiguration configuration,
         PublishDomainEventsInterceptor publishDomainEventsInterceptor)
    {
        _configuration = configuration;
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    public OnibiProDbContext CreateDbContext(string clientName)
    {
        if (!_contexts.ContainsKey(clientName))
        {
            var connectionString = _configuration.GetConnectionString($"SqlServerConnection_{clientName}");
            var optionsBuilder = new DbContextOptionsBuilder<OnibiProDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new OnibiProDbContext(optionsBuilder.Options, _publishDomainEventsInterceptor);
            _contexts[clientName] = context;
        }

        return _contexts[clientName];
    }
}