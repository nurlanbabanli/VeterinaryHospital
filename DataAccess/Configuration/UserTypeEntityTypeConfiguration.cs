using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class UserTypeEntityTypeConfiguration : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.HasIndex(x=>x.Id).IsUnique();
            builder.Property(x=>x.Id).HasColumnType("int").IsRequired();
            builder.Property(x => x.Name).HasColumnType("varchar(15)").IsRequired();
        }
    }
}
