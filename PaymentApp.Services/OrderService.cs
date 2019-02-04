using PaymentApp.Core.ComplexTypes;
using PaymentApp.Core.Enums;
using PaymentApp.Core.Interfaces;
using PaymentApp.Core.Interfaces.Services;
using PaymentApp.Core.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentApp.Services
{
    public class OrderService : IOrderService
    {

        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<OrderSummaryDto> GetOrderSummaryById(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderSummaryById(orderId);
            return order;
        }

        public async Task<CustomerDto> GetOrderCustomerAsync(int orderId)
        {
            return await _unitOfWork.OrderRepository.GetOrderCustomerAsync(orderId);

        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {

            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
            if (order == null)
                return false;

            try
            {
                OrderStatus oStatus;

                if (Enum.TryParse<OrderStatus>(status.ToUpper(), out oStatus))
                {
                    order.Status = oStatus;
                    await _unitOfWork.SaveChangesAsync();

                }
            }
            catch
            {
                
            }


            return false;
        }

    }
}
