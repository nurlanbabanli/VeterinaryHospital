using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class DoctorRatingEntityTypeConfiguration : IEntityTypeConfiguration<DoctorRating>
    {
        public void Configure(EntityTypeBuilder<DoctorRating> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(x=>x.Id).HasColumnType("int").UseIdentityColumn<int>().ValueGeneratedOnAdd().IsRequired();
            builder.Property(x => x.DoctorId).HasColumnType("int").IsRequired();
            builder.Property(x => x.PatientId).HasColumnType("int").IsRequired();
            builder.Property(x=>x.RatingFromPatient).HasColumnType("int").IsRequired();
            builder.Property(x => x.CommentFromPatient).HasColumnType("varchar(500)").IsRequired(false);
            builder.Property(x => x.ConfirmedRating).HasColumnType("boolean").IsRequired();
            builder.Property(x=>x.ConfirmedComment).HasColumnType("boolean").IsRequired();
        }
    }
}
