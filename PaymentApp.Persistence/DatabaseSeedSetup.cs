using PaymentApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using PaymentApp.Core.Enums;

namespace PaymentApp.Persistence
{
    public class DatabaseSeedSetup
    {

        public static void SeedDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    DateOfBirth = new DateTime(1980, 02, 23),
                    FirstName = "John",
                    LastName = "Smith"
                }
              );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Toy",
                    UnitPrice = 10.0
                },
                new Product
                {
                    Id = 2,
                    Name = "Candy",
                    UnitPrice = 0.5
                }
                );

            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    Total = 11.0,
                    Status = OrderStatus.NEW
                }
                );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 1,
                    Price = 10.0
                },

                new OrderItem
                {
                    Id = 2,
                    OrderId = 1,
                    ProductId = 2,
                    Quantity = 2,
                    Price = 1.0
                }
                );

        }
    }
}

