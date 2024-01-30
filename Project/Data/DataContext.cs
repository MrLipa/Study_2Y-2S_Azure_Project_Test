using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<UserMeal> UserMeals { get; set; }
        public DbSet<MealProduct> MealProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .HasKey(au => au.UserId);

            modelBuilder.Entity<UserMeal>()
                .HasKey(um => um.UserMealId);

            modelBuilder.Entity<UserMeal>()
                .HasOne(um => um.AppUser)
                .WithMany(au => au.UserMeals)
                .HasForeignKey(um => um.UserId);

            modelBuilder.Entity<UserMeal>()
                .HasOne(um => um.Meal)
                .WithMany(m => m.UserMeals)
                .HasForeignKey(um => um.MealId);

            modelBuilder.Entity<MealProduct>()
                .HasKey(mp => mp.MealProductId);

            modelBuilder.Entity<MealProduct>()
                .HasOne(mp => mp.Meal)
                .WithMany(m => m.MealProducts)
                .HasForeignKey(mp => mp.MealId);

            modelBuilder.Entity<MealProduct>()
                .HasOne(mp => mp.Product)
                .WithMany(p => p.MealProducts)
                .HasForeignKey(mp => mp.ProductId);
        }
    }
}
