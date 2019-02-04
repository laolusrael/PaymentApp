using PaymentApp.Core.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentApp.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<OrderSummaryDto> GetOrderSummaryById(int orderId);
        Task<CustomerDto> GetOrderCustomerAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
}
