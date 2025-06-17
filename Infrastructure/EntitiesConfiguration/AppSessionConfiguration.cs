using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class AppSessionConfiguration : IEntityTypeConfiguration<AppSession>
    {
        public void Configure(EntityTypeBuilder<AppSession> builder)
        {
            // Relationship configurations
            builder.HasMany(c => c.LogSessions)
                   .WithOne(c => c.AppSession)
                   .HasForeignKey(c => c.AppSessionId);

        }
    }
}
