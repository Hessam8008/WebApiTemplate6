using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(128)
            .IsUnicode(false);

        builder.Property(p => p.Error)
            .HasMaxLength(2048);

        builder.Property(p => p.OccurredOnUtc)
            .HasDefaultValue(DateTime.Now);
    }
}