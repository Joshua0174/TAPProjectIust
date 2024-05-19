using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DataAccessLayer
{
    public class MyDbContext : IdentityDbContext<AppUser>
    {
        private readonly string _windowsConnectionString = @"Server=.\SQLExpress;Database=ProjectDatabase1;Trusted_Connection=True;TrustServerCertificate=true";

        public DbSet<TestModel> TestModels { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CardPayment> cardPayments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(_windowsConnectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {   
            base.OnModelCreating(builder);

            //one-to-many intre produs si review
            builder.Entity<Product>()
            .HasMany(s => s.Reviews)            
            .WithOne(r => r.Product)                
            .HasForeignKey(r => r.ProductId);


            List<IdentityRole> roles = new List<IdentityRole> { 
               new IdentityRole
               {
                   Name="Admin",
                   NormalizedName="ADMIN"
               },
               new IdentityRole
               {
                   Name="User",
                   NormalizedName="USER"
               },
            };
            builder.Entity<IdentityRole>().HasData(roles);
            
            //one-to-one intre user si cart
            builder.Entity<AppUser>()
            .HasOne(u => u.Cart)
            .WithOne(c => c.AppUser)
            .HasForeignKey<Cart>(c => c.UserId);

            //many to many intre produse si cart (un produs poate fi in mai multe carturi, iar un cart poate avea mai multe produse)
            builder.Entity<CartItem>()
                .HasKey(ci => new { ci.CartId, ci.ProductId });

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);

            builder.Entity<CardPayment>()
           .HasOne(cp => cp.User)
           .WithOne(u => u.CardPayment)
           .HasForeignKey<CardPayment>(cp => cp.UserId);
        }
    }
}
