using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntitiesConfiguration
{
    public class ATMConfiguration : IEntityTypeConfiguration<ATM>
    {
        public void Configure(EntityTypeBuilder<ATM> builder)
        {
            // Relationship configurations
            builder.HasMany(c => c.ATMErrorCodes)
                   .WithOne(c => c.ATM)
                   .HasForeignKey(c => c.ATMId);

            builder.HasMany(c => c.Instructions)
                   .WithOne(c => c.ATM)
                   .HasForeignKey(c => c.ATMId);

            builder
                  .HasOne(c => c.ATMModel)
                  .WithMany(p => p.ATMs)
                  .HasForeignKey(s => s.ATMModelId);

            builder
                  .HasOne(c => c.ATMSoft)
                  .WithMany(p => p.ATMs)
                  .HasForeignKey(s => s.ATMModelId);
        }
    }
}
