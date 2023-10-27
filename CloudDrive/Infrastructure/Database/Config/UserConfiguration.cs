using Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("user_id");

        builder.Property(a => a.Username)
            .HasColumnName("username")
            .IsRequired(true);

        builder.HasMany(u => u.Accesses)
            .WithOne(a => a.User);
    }
}
