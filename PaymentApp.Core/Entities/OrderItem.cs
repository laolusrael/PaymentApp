using PaymentApp.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PaymentApp.Core.Entities
{
    public class OrderItem
    {
        [NotMapped]
        private Action<object, string> LazyLoader { get; set; }

        public OrderItem(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }

        public OrderItem()
        {

        }

        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        private Product _product;
        public virtual Product Product { get => LazyLoader.Load(this, ref _product); set => _product = value; }

    }
}
