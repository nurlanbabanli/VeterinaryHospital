using Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Id).HasColumnType("int").UseIdentityColumn<int>().ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.FirstName).HasColumnType("varchar(50)").IsRequired();
            builder.Property(x => x.LastName).HasColumnType("varchar(50)").IsRequired();
            builder.Property(x => x.Email).HasColumnType("varchar(50)").IsRequired();
            builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(x => x.PasswordSalt).HasColumnType("bytea").IsRequired();
            builder.Property(x => x.PasswordHash).HasColumnType("bytea").IsRequired();
        }
    }
}
