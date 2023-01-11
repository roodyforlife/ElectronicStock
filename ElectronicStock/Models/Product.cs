using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Display(Name = "Product name")]
        public string ProductTitle { get; set; }
        public string Description { get; set; }
        public int Cost { get; set; }
        [Display(Name = "Quantity in stock")]
        public int Quantity { get ;set; }
        [Display(Name = "Product discount")]
        public int Discount { get; set; }
        public byte[] Image { get; set; }
        [Display(Name = "Date created")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Available credit")]
        public bool CreditAvailable { get; set; }

    }
}
