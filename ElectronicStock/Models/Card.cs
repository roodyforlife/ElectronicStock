using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Models
{
    public class Card
    {
        public int CardId { get; set; }
        [Display(Name = "Date added")]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public ShopCard ShopCard { get; set; }
        public int ShopCardId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
