using Lab06.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lab06.DAL.EntitiesConfigurations
{
    public class UserTicketConfiguration : IEntityTypeConfiguration<UserTicket>
    {
        public void Configure(EntityTypeBuilder<UserTicket> builder)
        {
            builder.ToTable("UserTickets");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PurchaseTime)
                .HasDefaultValueSql("getdate()");

            builder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.UserTickets);

            builder.HasOne(x => x.ParkAttraction)
                .WithMany(x => x.UserTickets);
        }
    }
}
