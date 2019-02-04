using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Core.Shared.Dtos
{
    public class OrderSummaryDto
    {
        public OrderSummaryDto()
        {
            Items = new List<OrderItemDto>();
        }

        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Total { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
