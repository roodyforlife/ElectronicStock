using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Models
{
    public class ShopCard
    {
        public int ShopCardId { get; set; }
        [Display(Name = "Date created")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public User User { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Cart status")]
        public int Status { get; set; }
        public List<Card> Cards { get; set; }
    }
}
