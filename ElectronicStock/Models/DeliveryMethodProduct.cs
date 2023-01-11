using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Models
{
    public class DeliveryMethodProduct
    {
        public int DeliveryMethodProductId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public int DeliveryMethodId { get; set; }
    }
}
