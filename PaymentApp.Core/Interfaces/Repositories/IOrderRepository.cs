using PaymentApp.Core.Entities;
using PaymentApp.Core.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentApp.Core.Interfaces.Repositories
{
    public interface IOrderRepository:IRepository<Order>
    {
        Task<OrderSummaryDto> GetOrderSummaryById(int orderId);
        Task<CustomerDto> GetOrderCustomerAsync(int orderId);
    }
}
