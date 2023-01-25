using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SafeBank.Models;

namespace SafeBank.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Transaction> Transactions { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hashed = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser { Id = "1a7addfc-89fa-4f4e-8171-3b07035b6085", FullName = "John Doe", Balance = 0, PESEL = "22222222222", Email = "johndoe@example.com", NormalizedEmail = "JOHNDOE@EXAMPLE.COM", UserName = "johndoe@example.com", NormalizedUserName = "JOHNDOE@EXAMPLE.COM", PasswordHash = hashed.HashPassword(null, "Administrator1!"), DateOfBirth = new DateTime(1999, 5, 15) },
                new AppUser { Id = "51dda72b-3636-4b29-b09c-62da288b7445", FullName = "Lucas Doe", Balance = 500_000,  PESEL = "22222222222", Email = "lucasdoe@example.com", NormalizedEmail = "LUCASDOE@EXAMPLE.COM", UserName = "lucasdoe@example.com", NormalizedUserName = "LUCASDOE@EXAMPLE.COM", PasswordHash = hashed.HashPassword(null, "Administrator2!"), DateOfBirth = new DateTime(2001, 1, 12) },
                new AppUser { Id = "67f0426a-a053-40e4-9f23-6cb5d7d84cd6", FullName = "Thomas Doe", Balance = 10_000_000,  PESEL = "22222222222", Email = "thomasdoe@example.com", NormalizedEmail = "THOMASDOE@EXAMPLE.COM", UserName = "thomasdoe@example.com", NormalizedUserName = "THOMASDOE@EXAMPLE.COM", PasswordHash = hashed.HashPassword(null, "Administrator3!"), DateOfBirth = new DateTime(2006, 3, 3) }
                );

            modelBuilder.Entity<AppUser>().Property(prop => prop.AccountNumber).UseIdentityColumn(10_000, 1);

            /*modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "0773fad5-d7e7-47e1-9ef8-6fad8120694f", Name = "admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "5d1880d4-bc4a-4873-9043-5320c72b35c0", Name = "mechanik", NormalizedName = "MECHANIK" }
                );*/

            modelBuilder.Entity<Transaction>().ToTable("Transactions");

            var decimalProps = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
