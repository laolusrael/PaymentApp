using PaymentApp.Core.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentApp.UI.Models.Interfaces
{
    public interface IPaymentViewModel
    {
        Task<bool> ConfirmCustomerBirthDateAsync(DateConfirmationDto model);
        Task<OrderSummaryDto> GetOrderSummaryAsync(int orderId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
}
