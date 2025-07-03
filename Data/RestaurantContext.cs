using Microsoft.EntityFrameworkCore;
using SmallRestaurantApp.Models;
using System;

namespace SmallRestaurantApp.Data
{
  public class RestaurantContext : DbContext
  {
    public RestaurantContext(DbContextOptions<RestaurantContext> options) :
      base(options)
    {
    }

    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Dish → Comments (1:N)
      modelBuilder.Entity<Dish>()
        .HasMany(d => d.Comments)
        .WithOne(r => r.Dish)
        .HasForeignKey(r => r.DishId);

      // User → Orders (1:N)
      modelBuilder.Entity<User>()
        .HasMany(u => u.Orders)
        .WithOne(o => o.User)
        .HasForeignKey(o => o.UserId);

      // User → Addresses (1:N)
      modelBuilder.Entity<User>()
        .HasMany(u => u.Addresses)
        .WithOne(a => a.User)
        .HasForeignKey(a => a.UserId);

      // Order → Items (1:N)
      modelBuilder.Entity<Order>()
        .HasMany(o => o.Items)
        .WithOne(i => i.Order)
        .HasForeignKey(i => i.OrderId);

      // Order → DeliveryAddress (0..1)
      modelBuilder.Entity<Order>()
        .HasOne(o => o.DeliveryAddress)
        .WithMany()
        .HasForeignKey(o => o.DeliveryAddressId);

      modelBuilder.Entity<Dish>().HasData(
        new Dish
        {
          DishId = 1, Name = "Lasagne",
          Description = "Lasagne: layers of pasta, rich sauce, and cheese, oven-baked.",
          Price = 9.00m, ImagePath = "images/lasagne.png"
        },
        new Dish
        {
          DishId = 2, Name = "Ravioli",
          Description =
            "Ravioli: stuffed pasta pillows filled with savory goodness, gently cooked and sauced.",
          Price = 7.00m, ImagePath = "images/ravioli.png"
        },
        new Dish
        {
          DishId = 3, Name = "Panna cotta",
          Description =
            "Panna cotta: silky chilled cream dessert with a hint of vanilla.",
          Price = 5.00m, ImagePath = "images/pannacotta.png"
        }
      );

      modelBuilder.Entity<User>().HasData(
        new User { UserId = 1, Name = "Albert" },
        new User { UserId = 2, Name = "Nicola" }
      );

      modelBuilder.Entity<Address>().HasData(
        new Address
        {
          AddressId = 1, UserId = 1, Street = "4 Piastri St",
          City = "Berehove", PostalCode = "71615"
        },
        new Address
        {
          AddressId = 2, UserId = 2, Street = "41 Imola St",
          City = "Berehove", PostalCode = "71604"
        }
      );
    }
  }

  public static class DbSeeder
  {
    public static void Seed(RestaurantContext context)
    {
      if (!context.Dishes.Any())
      {
        context.Dishes.AddRange(
          new Dish
          {
            DishId = 1, Name = "Lasagne",
            Description = "Lasagne: layers of pasta, rich sauce, and cheese, oven-baked.",
            Price = 9.00m, ImagePath = "images/lasagne.png"
          },
          new Dish
          {
            DishId = 2, Name = "Ravioli",
            Description = "Ravioli: stuffed pasta pillows filled with savory goodness, gently cooked and sauced.",
            Price = 7.00m, ImagePath = "images/ravioli.png"
          },
          new Dish
          {
            DishId = 3, Name = "Panna cotta",
            Description =
              "Panna cotta: silky chilled cream dessert with a hint of vanilla.",
            Price = 5.00m, ImagePath = "images/pannacotta.png"
          }
        );
      }

      if (!context.Users.Any())
      {
        context.Users.AddRange(
          new User { UserId = 1, Name = "Albert" },
          new User { UserId = 2, Name = "Nicola" }
        );
      }

      context.SaveChanges();

      if (!context.Addresses.Any())
      {
        context.Addresses.AddRange(
          new Address
          {
            AddressId = 1, UserId = 1, Street = "4 Piastri St",
            City = "Berehove", PostalCode = "71615"
          },
          new Address
          {
            AddressId = 2, UserId = 2, Street = "41 Imola St",
            City = "Berehove", PostalCode = "71604"
          }
        );
      }

      context.SaveChanges(); // ensure addresses exist before orders

      if (!context.Orders.Any())
      {
        context.Orders.Add(new Order
        {
          UserId = 1,
          OrderDate = DateTime.UtcNow.AddDays(-1),
          Status = OrderStatus.Delivered,
          IsDelivery = true,
          DeliveryAddressId = 1,
          DeliveryFee = 2.00m,
          TotalPrice = 11.00m
        });

        context.SaveChanges();

        context.OrderItems.Add(new OrderItem
        {
          OrderItemId = 1,
          OrderId = 1,
          DishId = 1,
          Quantity = 1,
          UnitPrice = 9.00m,
          LineTotal = 9.00m
        });
      }

      if (!context.Comments.Any())
      {
        context.Comments.AddRange(
          new Comment
          {
            ReviewId = 1,
            DishId = 1,
            UserId = 1,
            Rating = 5,
            Text = "Well Well Well👌",
            ReviewDate = DateTime.UtcNow.AddDays(-1)
          },
          new Comment
          {
            ReviewId = 2,
            DishId = 1,
            UserId = 2,
            Rating = 5,
            Text = "Perfect💫",
            ReviewDate = DateTime.UtcNow.AddDays(-2)
          }
        );
      }

      context.SaveChanges();
    }
  }
}