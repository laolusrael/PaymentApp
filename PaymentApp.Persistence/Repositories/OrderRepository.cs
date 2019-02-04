using PaymentApp.Core.Entities;
using PaymentApp.Core.Interfaces.Repositories;
using PaymentApp.Core.Shared.Dtos;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PaymentApp.Persistence.Repositories
{
    public class OrderRepository : Repository<Order, PaymentAppContext>, IOrderRepository
    {
        public OrderRepository(PaymentAppContext dataContext) : base(dataContext)
        {
        }

        public Task<CustomerDto> GetOrderCustomerAsync(int orderId)
        {
            var query = from order in DatabaseContext.Orders
                      join customer in DatabaseContext.Customers on order.CustomerId equals customer.Id

                      where order.Id == orderId

                      select new CustomerDto
                      {

                          DateOfBirth = customer.DateOfBirth,
                          FirstName = customer.FirstName,
                          LastName = customer.LastName,
                          Id = customer.Id
                      };


            return query.AsNoTracking().FirstOrDefaultAsync();
        }

        public Task<OrderSummaryDto> GetOrderSummaryById(int orderId)
        {
            var query = from order in DatabaseContext.Orders
                        join customer in DatabaseContext.Customers on order.CustomerId equals customer.Id

                        let items = DatabaseContext.OrderItems.Where(item => item.OrderId == order.Id)

                        where order.Id == orderId

                        select new OrderSummaryDto
                        {

                            DateOfBirth = customer.DateOfBirth,
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            Id = order.Id,
                            Status = order.Status.ToString(),
                            Total = order.Total,
                            Items = items.Any() ? items.Select(i => new OrderItemDto
                            {
                                Id = i.Id,
                                Price = i.Price,
                                Quantity = i.Quantity,
                                ProductId = i.ProductId,
                                Product = i.Product != null ? i.Product.Name : ""
                            }).ToList()
                            : new List<OrderItemDto>()
                        };

            return query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
