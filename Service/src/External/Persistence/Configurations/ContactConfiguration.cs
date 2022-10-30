using System.Drawing;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .HasConversion(p => p.Value, p => ContactTitle.Create(p))
            .HasMaxLength(ContactTitle.MaxLength)
            .IsRequired();

        builder.Property(p => p.Caption)
            .HasConversion(p => p.Value, p => ContactCaption.Create(p))
            .HasMaxLength(ContactCaption.MaxLength)
            .IsRequired();

        builder.Property(p => p.InternalNumber)
            .HasConversion(p => p.Value, p => InternalNumber.Create(p))
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(p => p.Building)
            .HasConversion(p => (byte) p, p => (Building) p)
            .HasColumnType("tinyint")
            .HasDefaultValue(Building.Unknown)
            .IsRequired();

        builder.Property(p => p.BackColor)
            .HasConversion(p => p.ToArgb(), p => Color.FromArgb(p))
            .HasColumnType("int")
            .HasDefaultValue(Color.Empty)
            .IsRequired();

        builder.Property(p => p.EmployeeCode)
            .HasConversion(p => p!.Value, p => EmployeeCode.Create(p))
            .IsRequired(false)
            .HasColumnType("int");

        //builder.Property(p => p.BirthDate)
        //    .HasColumnType("date")
        //    .HasConversion(p => p.ToDateTime(TimeOnly.MinValue), p => new DateOnly(p.Year, p.Month, p.Day));
    }
}