using Lab06.DAL.Entities;
using Lab06.DAL.EntitiesConfigurations;
using Lab06.DAL.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab06.DAL.Context
{
    public class ParkContext : IdentityDbContext<ApplicationUser>
    {
        public ParkContext(DbContextOptions<ParkContext> options)
            : base(options)
        { }

        public DbSet<ParkAttraction> ParkAttractions { get; set; }

        public DbSet<AttractionImage> AttractionImages { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        
        public DbSet<IdentityRole> IdentityRoles { get; set; }

        public DbSet<UserTicket> UserTicket { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApplicationUserConfiguaration());
            modelBuilder.ApplyConfiguration(new ParkAttractionConfiguration());
            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new UserTicketConfiguration());

            modelBuilder.Seed();
        }
    }
}
