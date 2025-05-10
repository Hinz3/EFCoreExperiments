using EFCoreExperiments.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFCoreExperiments.DataContext.Contexts;

public class MainContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
