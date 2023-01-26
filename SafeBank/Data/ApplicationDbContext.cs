using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SafeBank.Models;

namespace SafeBank.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hashed = new PasswordHasher<User>();
            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<Notification>().ToTable("Notifications");

            modelBuilder.Entity<User>().HasData(
                new User { Id = "1a7addfc-89fa-4f4e-8171-3b07035b6085", FullName = "John Doe", Email = "johndoe@example.com", NormalizedEmail = "JOHNDOE@EXAMPLE.COM", UserName = "johndoe@example.com", NormalizedUserName = "JOHNDOE@EXAMPLE.COM", PasswordHash = hashed.HashPassword(null, "Administrator1!"), DateOfBirth = new DateTime(1999, 5, 15) },
                new User { Id = "51dda72b-3636-4b29-b09c-62da288b7445", FullName = "Lucas Doe", Email = "lucasdoe@example.com", NormalizedEmail = "LUCASDOE@EXAMPLE.COM", UserName = "lucasdoe@example.com", NormalizedUserName = "LUCASDOE@EXAMPLE.COM", PasswordHash = hashed.HashPassword(null, "Administrator2!"), DateOfBirth = new DateTime(2001, 1, 12) },
                new User { Id = "67f0426a-a053-40e4-9f23-6cb5d7d84cd6", FullName = "Thomas Doe", Email = "thomasdoe@example.com", NormalizedEmail = "THOMASDOE@EXAMPLE.COM", UserName = "thomasdoe@example.com", NormalizedUserName = "THOMASDOE@EXAMPLE.COM", PasswordHash = hashed.HashPassword(null, "Administrator3!"), DateOfBirth = new DateTime(2006, 3, 3) }
            );

            modelBuilder.Entity<Account>().HasData(
                new Account { Id = 1, IBAN = "10001", Balance = 0, OwnerId = "1a7addfc-89fa-4f4e-8171-3b07035b6085" },
                new Account { Id = 2, IBAN = "10002", Balance = 500_000, OwnerId = "51dda72b-3636-4b29-b09c-62da288b7445" },
                new Account { Id = 3, IBAN = "10003", Balance = 1_000_000, OwnerId = "67f0426a-a053-40e4-9f23-6cb5d7d84cd6" }
                );


            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = "ea215411-5883-4cf2-987f-5458599481d8",
                    Name = "Admin"
                }
                );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>()
                {
                    RoleId = "ea215411-5883-4cf2-987f-5458599481d8",
                    UserId = "51dda72b-3636-4b29-b09c-62da288b7445"
                });



            var decimalProps = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
