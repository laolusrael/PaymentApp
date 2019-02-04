using Microsoft.EntityFrameworkCore;
using PaymentApp.Core.Entities;
using System.Threading;
using System.Threading.Tasks;
using static PaymentApp.Persistence.Helpers.Converters;

namespace PaymentApp.Persistence
{
    public class PaymentAppContext : DbContext
    {
        /// <summary>
        /// Use this constructor when calling from WebApi service setup
        /// </summary>
        /// <param name="options"></param>
        public PaymentAppContext(DbContextOptions<PaymentAppContext> options) : base(options) {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DatabaseSeedSetup.SeedDatabase(modelBuilder);
        }




        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}

