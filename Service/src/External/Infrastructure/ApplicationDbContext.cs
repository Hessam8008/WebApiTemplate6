using System.Diagnostics;
using Domain.Entities;
using Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("server=10.10.1.102;uid=sa;pwd=3592;database=WebApiDB")
            .LogTo(l => Debug.WriteLine(l), LogLevel.Information);

        base.OnConfiguring(optionsBuilder);
    }
}