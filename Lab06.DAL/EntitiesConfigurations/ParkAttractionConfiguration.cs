using Lab06.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lab06.DAL.EntitiesConfigurations
{
    public class ParkAttractionConfiguration : IEntityTypeConfiguration<ParkAttraction>
    {
        public void Configure(EntityTypeBuilder<ParkAttraction> builder)
        {
            builder.ToTable("ParkAttractions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasDefaultValue(0M)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            builder.HasOne(a => a.AttractionImage)
                .WithOne(x => x.ParkAttraction)
                .HasForeignKey<AttractionImage>(a => a.AttractionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
