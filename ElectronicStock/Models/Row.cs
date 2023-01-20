using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Models
{
    public class Row
    {
        public int RowId { get; set; }
        [Display(Name = "Row number")]
        public int RowNumber { get; set; }
        [Display(Name = "Place start number")]
        public int PlaceStartNumber { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
