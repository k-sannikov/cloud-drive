using Domain.AccessSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config;

public class AccessConfiguration : IEntityTypeConfiguration<Access>
{
    public void Configure(EntityTypeBuilder<Access> builder)
    {
        builder.ToTable("access");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("access_id");

        builder.Property(a => a.UserId)
            .HasColumnName("user_id")
            .IsRequired(true);

        builder.Property(a => a.NodeId)
            .HasColumnName("node_id")
            .IsRequired(true);

        builder.Property(a => a.IsOwner)
            .HasColumnName("is_owner")
            .IsRequired(true);

        builder.HasOne(a => a.User)
            .WithMany(u => u.Accesses);
    }
}
