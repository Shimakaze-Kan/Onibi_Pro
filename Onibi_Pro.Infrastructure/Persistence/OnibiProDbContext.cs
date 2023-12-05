using Microsoft.EntityFrameworkCore;

using Onibi_Pro.Domain.MenuAggregate;

namespace Onibi_Pro.Infrastructure.Persistence;
internal sealed class OnibiProDbContext : DbContext
{
    public OnibiProDbContext(DbContextOptions<OnibiProDbContext> options)
        : base(options)
    {
    }

    public DbSet<Menu> Menus { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnibiProDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
