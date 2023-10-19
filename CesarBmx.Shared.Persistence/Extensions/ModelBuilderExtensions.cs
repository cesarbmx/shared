using MassTransit;
using Microsoft.EntityFrameworkCore;


namespace CesarBmx.Shared.Persistence.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void UseSingularTableNames(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Use the entity name instead of the Context.DbSet<T> name
                // refs https://docs.microsoft.com/en-us/ef/core/modeling/relational/tables#conventions
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            }
        }
        public static void UseMasstransitOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStateEntity(x=>x.Metadata.SetSchema("Masstransit"));
            modelBuilder.AddOutboxMessageEntity(x => x.Metadata.SetSchema("Masstransit"));
            modelBuilder.AddOutboxStateEntity(x => x.Metadata.SetSchema("Masstransit"));
        }
    }
}
