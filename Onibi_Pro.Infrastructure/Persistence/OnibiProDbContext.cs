﻿using Microsoft.EntityFrameworkCore;

using Onibi_Pro.Domain.Common.Interfaces;
using Onibi_Pro.Infrastructure.Persistence.Interceptors;

namespace Onibi_Pro.Infrastructure.Persistence;
internal sealed class OnibiProDbContext : DbContext
{
    private readonly PublishDomainEventsInterceptor _publishDomainEvents;

    public OnibiProDbContext(DbContextOptions<OnibiProDbContext> options,
        PublishDomainEventsInterceptor publishDomainEvents)
        : base(options)
    {
        _publishDomainEvents = publishDomainEvents;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<List<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(OnibiProDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEvents);
        base.OnConfiguring(optionsBuilder);
    }
}
