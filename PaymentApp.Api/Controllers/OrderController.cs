using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentApp.Core.Interfaces.Services;
using PaymentApp.Core.Shared.Dtos;

namespace PaymentApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet, Route("{orderId}")]
        public async Task<OrderSummaryDto> GetOrderSummaryAsync(int orderId)
        {
            return await _orderService.GetOrderSummaryById(orderId);
        }


        [HttpPost, Route("confirmCustomerDoB")]
        public async Task<bool> ConfirmOrderCustomerBirthDateAsync(DateConfirmationDto model)
        {

            var customer = await _orderService.GetOrderCustomerAsync(model.OrderId);
            if (customer == null)
                return false;

            return customer.DateOfBirth.Date == model.DateOfBirth.Date;

        }
        [HttpGet, Route("updateStatus/{orderId}/{status}")]
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            return await _orderService.UpdateOrderStatusAsync(orderId, status);

        }

    }
}