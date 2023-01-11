using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Models
{
    public class DeliveryMethod
    {
        public int DeliveryMethodId { get; set; }
        [Display(Name = "Delivery method")]
        public string MethodName { get; set; }
        [Display(Name = "Date created")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
