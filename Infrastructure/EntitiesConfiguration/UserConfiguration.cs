using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Relationship configurations
            builder.HasMany(c => c.AppSessions)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId);

            builder
                .HasOne(c => c.Bank)
                .WithMany(s => s.Users)
                .HasForeignKey(s => s.BankId);

            builder
                .HasOne(c => c.Region)
                .WithMany(p => p.Users)
                .HasForeignKey(s => s.RegionId);
        }
    }
}
