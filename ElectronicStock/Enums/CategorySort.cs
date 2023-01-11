using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Enums
{
    public enum CategorySort
    {
        [Display(Name = "Name a-z")]
        NameAsc,
        [Display(Name = "Name z-a")]
        NameDesc,
        [Display(Name = "New first")]
        DateAsc,
        [Display(Name = "Old first")]
        DateDesc
    }
}
