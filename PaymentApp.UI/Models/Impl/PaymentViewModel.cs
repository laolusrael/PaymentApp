using PaymentApp.Core.Shared.Dtos;
using PaymentApp.UI.Helpers;
using PaymentApp.UI.Models.Interfaces;
using System.Threading.Tasks;

namespace PaymentApp.UI.Models.Impl
{
    public class PaymentViewModel:IPaymentViewModel
    {
        private readonly IRestClient _client;
        public PaymentViewModel(IRestClient rclient)
        {
            _client = rclient;
        }


        public  async Task<bool> ConfirmCustomerBirthDateAsync(DateConfirmationDto model)
        {

            var result = await _client.Post<bool, DateConfirmationDto>("api/order/confirmCustomerDoB", model);
            return result;

        }

        public async Task<OrderSummaryDto> GetOrderSummaryAsync(int orderId)
        {
            var result = await _client.Get<OrderSummaryDto>($"api/order/{orderId}");
            return result;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var result = await _client.Get<bool>($"api/order/updateStatus/{orderId}/{status}");
            return result;
        }
    }
}
