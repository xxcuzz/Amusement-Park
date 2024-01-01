using Lab06.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lab06.DAL.EntitiesConfigurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<AttractionImage>
    {
        public void Configure(EntityTypeBuilder<AttractionImage> builder)
        {
            builder.ToTable("Images");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(80)
                .IsRequired();
        }
    }
}
