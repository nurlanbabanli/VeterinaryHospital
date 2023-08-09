using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    public class UserOperationClaimEntityTypeConfiguration : IEntityTypeConfiguration<UserOperationClaim>
    {
        public void Configure(EntityTypeBuilder<UserOperationClaim> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasIndex(c => c.Id).IsUnique();
            builder.Property(c => c.Id).HasColumnType("int").UseIdentityColumn<int>().ValueGeneratedOnAdd().IsRequired(true);
            builder.Property(c => c.UserId).HasColumnType("int").IsRequired(true);
            builder.Property(c => c.OperationClaimId).HasColumnType("int").IsRequired(true);
        }
    }
}
