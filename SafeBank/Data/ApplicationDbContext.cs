using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SafeBank.Models;

namespace SafeBank.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hashed = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = "1", FullName= "Admin Admin", PESEL = "11111111111", Email = "admin@admin.pl", NormalizedEmail = "ADMIN@ADMIN.PL", UserName = "Admin", NormalizedUserName = "ADMIN", PasswordHash = hashed.HashPassword(null, "Administrator1!"), DateOfBirth = new DateTime(2000, 1, 1) }
                );

            /*modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "0773fad5-d7e7-47e1-9ef8-6fad8120694f", Name = "admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "5d1880d4-bc4a-4873-9043-5320c72b35c0", Name = "mechanik", NormalizedName = "MECHANIK" }
                );*/
            base.OnModelCreating(modelBuilder);
        }
    }
}
