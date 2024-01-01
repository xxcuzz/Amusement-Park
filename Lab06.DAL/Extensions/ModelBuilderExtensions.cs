using Lab06.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;

namespace Lab06.DAL.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static string resourceFolderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttractionImage>().HasData(
                new AttractionImage()
                {
                    Id = 1,
                    Name = "Carousel",
                    Payload = File.ReadAllBytes(Path.Combine(resourceFolderPath, "Resources", "carousel.png")),
                    AttractionId = 1,
                },
                new AttractionImage()
                {
                    Id = 2,
                    Name = "Fair Ship",
                    Payload = File.ReadAllBytes(Path.Combine(resourceFolderPath, "Resources", "fair-ship.png")),
                    AttractionId = 2,
                },
                new AttractionImage()
                {
                    Id = 3,
                    Name = "Bumper Car",
                    Payload = File.ReadAllBytes(Path.Combine(resourceFolderPath, "Resources", "bumper-car.png")),
                    AttractionId = 3,
                },
                new AttractionImage()
                {
                    Id = 4,
                    Name = "Spinning Swing",
                    Payload = File.ReadAllBytes(Path.Combine(resourceFolderPath, "Resources", "spinning-swing.png")),
                    AttractionId = 4,
                },
                new AttractionImage()
                {
                    Id = 5,
                    Name = "Roller Coaster",
                    Payload = File.ReadAllBytes(Path.Combine(resourceFolderPath, "Resources", "rollercoaster.png")),
                    AttractionId = 5,
                }
            );

            modelBuilder.Entity<ParkAttraction>().HasData(
                new ParkAttraction
                {
                    Id = 1,
                    Name = "Carousel",
                    Price = 4.5M,
                },
                new ParkAttraction
                {
                    Id = 2,
                    Name = "Flying Chairs",
                    Price = 3.0M,
                },
                new ParkAttraction
                {
                    Id = 3,
                    Name = "Bumper Car",
                    Price = 5.0M,
                },
                new ParkAttraction
                {
                    Id = 4,
                    Name = "Roll-O-Plane",
                    Price = 3.5M,
                },
                new ParkAttraction
                {
                    Id = 5,
                    Name = "Roller Coaster",
                    Price = 5.5M,
                }
            );

            // Seeding administrator
            modelBuilder.Entity<IdentityRole>()
                .HasData(new IdentityRole 
                { 
                    Id = "41e31b83-c772-415e-bd8a-30eeb4784216", 
                    Name = "Administrator", 
                    NormalizedName = "ADMINISTRATOR".ToUpper() 
                });

            var hasher = new PasswordHasher<IdentityUser>();

            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "76016f0b-8600-4309-9581-1a84264b8e45",
                    FirstName = "Bob",
                    Surname = "Bobov",
                    Email = "parkadmin2021@gmail.com",
                    UserName = "parkadmin2021",
                    NormalizedUserName = "PARKADMIN2021",
                    NormalizedEmail = "PARKADMIN2021@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "Pass+w0rd")
                }
            );


            //Seeding the relation between our user and role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "41e31b83-c772-415e-bd8a-30eeb4784216",
                    UserId = "76016f0b-8600-4309-9581-1a84264b8e45"
                }
            );

            // Seeding Employee
            modelBuilder.Entity<IdentityRole>()
                .HasData(new IdentityRole
                {
                    Id = "24952ebf-62d0-42d5-961e-a7123b14009e", 
                    Name = "Employee", 
                    NormalizedName = "EMPLOYEE".ToUpper()
                });

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "dd0b71da-5f66-4bfb-abf4-47030635f9a0",
                    FirstName = "Roma",
                    Surname = "Savin",
                    Email = "romasavin2021@gmail.com",
                    UserName = "romasavin2021",
                    NormalizedUserName = "ROMASAVIN2021",
                    NormalizedEmail = "ROMASAVIN2021@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "Pass+w0rd")
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "24952ebf-62d0-42d5-961e-a7123b14009e",
                    UserId = "dd0b71da-5f66-4bfb-abf4-47030635f9a0"
                }
            );
        }
    }
}
