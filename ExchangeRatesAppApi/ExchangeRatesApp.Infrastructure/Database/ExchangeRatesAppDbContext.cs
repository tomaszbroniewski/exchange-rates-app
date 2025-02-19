using ExchangeRatesApp.Domain.UserAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExchangeRatesApp.Infrastructure.Database
{
    public class ExchangeRatesAppDbContext : DbContext
    {
#pragma warning disable CS8618 // Entity Framework takes care of that.
        public ExchangeRatesAppDbContext(DbContextOptions<ExchangeRatesAppDbContext> options) : base(options)
#pragma warning restore CS8618 // Entity Framework takes care of that.
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExchangeRatesAppDbContext).Assembly);
        }

        public static void Initialize(ExchangeRatesAppDbContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                dbContext.Users.AddRange(
                    User.Create("user", "qwerty", new PasswordHasher<User>())
                );
            }

            dbContext.SaveChanges();
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
