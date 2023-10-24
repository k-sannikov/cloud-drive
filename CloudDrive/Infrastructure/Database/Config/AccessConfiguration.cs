using Domain.AccessService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Config;

public class AccessConfiguration : IEntityTypeConfiguration<Access>
{
    public void Configure(EntityTypeBuilder<Access> builder)
    {
        builder.ToTable("user_access");

        builder.HasKey(a => new { a.UserId, a.NodeId });

        builder.Property(a => a.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(a => a.NodeId).HasColumnName("node_id").IsRequired();

        builder.Property(a => a.IsOwner).HasColumnName("is_owner").IsRequired();

        builder.HasOne(a => a.User).WithMany(u => u.Accesses).HasForeignKey(a => a.UserId);
        builder.HasOne(a => a.Node).WithMany(n => n.Accesses).HasForeignKey(a => a.NodeId);
    }
}
