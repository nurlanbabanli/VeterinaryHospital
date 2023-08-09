using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class OperationClaimEntityTypeConfiguration : IEntityTypeConfiguration<OperationClaim>
    {
        public void Configure(EntityTypeBuilder<OperationClaim> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.Id).IsUnique();
            builder.Property(o => o.Id).HasColumnType("int").ValueGeneratedOnAdd().UseIdentityColumn<int>().IsRequired(true);
            builder.Property(o => o.Name).HasColumnType("varchar(50)").IsRequired(true);
            builder.HasIndex(o => o.Name).IsUnique();

            builder.HasData(
                new OperationClaim { Id=1, Name = "admin" },
                new OperationClaim { Id=2, Name = "user" },
                new OperationClaim { Id=3, Name="superUser" }
                );
        }
    }
}
