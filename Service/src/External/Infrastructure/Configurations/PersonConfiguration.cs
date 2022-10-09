using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Email)
            .HasConversion(p => p.Value, p => Email.Create(p))
            .HasMaxLength(Email.MaxLength)
            .IsRequired();

        builder.Property(p => p.NationCode)
            .HasConversion(p => p.Value, p => NationalCode.Create(p))
            .HasMaxLength(NationalCode.Length)
            .IsRequired();

        builder.Property(p => p.FirstName)
            .HasConversion(p => p.Value, p => FirstName.Create(p))
            .HasMaxLength(FirstName.MaxLength)
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasConversion(p => p.Value, p => LastName.Create(p))
            .HasMaxLength(LastName.MaxLength)
            .IsRequired();

        builder.Property(p => p.BirthDate)
            .HasConversion(p => p.ToDateTime(TimeOnly.MinValue), p => new DateOnly(p.Year, p.Month, p.Day));
    }
}