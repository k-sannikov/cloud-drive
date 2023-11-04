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
            .HasColumnName("user_id")
            .ValueGeneratedNever();

        builder.Property(a => a.Username)
            .HasColumnName("username")
            .IsRequired(true);

        builder.Property(a => a.Password)
            .HasColumnName("password")
            .IsRequired(true);

        builder.Property(a => a.RefreshToken)
            .HasColumnName("refresh_token")
            .HasDefaultValue("")
            .IsRequired(true);

        builder.Property(a => a.RefreshTokenExpiryTime)
            .HasColumnName("refresh_token_expiry_time")
            .IsRequired(true)
            .HasDefaultValueSql("now()");

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.HasMany(u => u.Accesses)
            .WithOne(a => a.User);
    }
}
