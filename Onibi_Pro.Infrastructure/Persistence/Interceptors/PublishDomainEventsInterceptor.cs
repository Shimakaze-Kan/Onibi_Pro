using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

using Onibi_Pro.Domain.Common.Interfaces;

namespace Onibi_Pro.Infrastructure.Persistence.Interceptors;
public sealed class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    public PublishDomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        PublishDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public async override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData,
        int result, CancellationToken cancellationToken = default)
    {
        await PublishDomainEvents(eventData.Context);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return;
        }

        var entitiesWithDomainEvents = dbContext.ChangeTracker.Entries<IHasDomainEvent>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .ToList();

        var domainEvents = entitiesWithDomainEvents.SelectMany(entry => entry.DomainEvents).ToList();

        entitiesWithDomainEvents.ForEach(entry => entry.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }
    }
}
