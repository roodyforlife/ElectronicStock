using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Empty field")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Invalid length, it should be between 5 and 20 characters")]
        public string Name { get; set; }
        [Display(Name = "Date created")]
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
