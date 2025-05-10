using EFCoreExperiments.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreExperiments.DataContext.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Firstname).HasColumnType("nvarchar(500)");
        builder.Property(x => x.Lastname).HasColumnType("nvarchar(500)");
        builder.Property(x => x.EmailAddress).HasColumnType("nvarchar(1000)");
        builder.Property(x => x.Created).HasColumnType("datetime");
        builder.Property(x => x.Updated).HasColumnType("datetime");
    }
}
