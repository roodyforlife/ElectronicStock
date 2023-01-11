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
        [Required(ErrorMessage = "Empty field")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Invalid length, it should be between 5 and 20 characters")]
        public string ProductTitle { get; set; }
        [Required(ErrorMessage = "Empty field")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Invalid length, it should be between 5 and 200 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Empty field")]
        public int Cost { get; set; }
        [Display(Name = "Quantity in stock")]
        [Required(ErrorMessage = "Empty field")]
        public int Quantity { get ;set; }
        [Required(ErrorMessage = "Empty field")]
        [Display(Name = "Product discount")]
        [Range(0, 99, ErrorMessage = "Must be between 0 and 99")]
        public int Discount { get; set; }
        public byte[] Image { get; set; }
        [Display(Name = "Date created")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Display(Name = "Available credit")]
        public bool CreditAvailable { get; set; }
        public List<ProductCategory> productCategories { get; set; }

    }
}
