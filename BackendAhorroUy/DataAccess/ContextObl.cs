using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ContextObl : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Market> Market { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductMarket> ProductsMarkets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        public ContextObl(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserSession>()
                .HasKey(us => us.Token);

            modelBuilder.Entity<ProductMarket>()
               .HasOne<Product>(ts => ts.Product)
               .WithMany(ts => ts.ProductsMarkets)
               .HasForeignKey(ts => ts.ProductId);

            modelBuilder.Entity<ProductMarket>()
                .HasOne(cat => cat.Market)
                .WithMany(cat => cat.ProductsMarkets)
                .HasForeignKey(cat => cat.MarketId);

            modelBuilder.Entity<ProductMarket>()
                .HasKey(lp => new { lp.ProductId, lp.MarketId });

            modelBuilder.Entity<Token>()
                .HasKey(token => new { token.TokenValue });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
