using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentApp.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }
    }
}
