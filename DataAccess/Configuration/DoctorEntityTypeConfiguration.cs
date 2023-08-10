using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class DoctorEntityTypeConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(x=>x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x => x.Id).HasColumnType("int").UseIdentityColumn<int>().ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.IsConfirmed).HasColumnType("boolean");
            builder.Property(x => x.Raiting).HasColumnType("float").IsRequired();
            builder.Property(x => x.ImageUrl).HasColumnType("varchar(100)").IsRequired(false);
        }
    }
}
