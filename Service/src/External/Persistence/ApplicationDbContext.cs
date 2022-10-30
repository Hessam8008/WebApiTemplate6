using System.Diagnostics;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Outbox;

namespace Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Contact>? Contacts { get; set; }

    public DbSet<OutboxMessage>? OutboxMessages { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
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