using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Core.Shared.Dtos
{
    public class AddOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
