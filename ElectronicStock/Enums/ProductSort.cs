using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Enums
{
    public enum ProductSort
    {
        [Display(Name = "Title a-z")]
        TitleAsc,
        [Display(Name = "Title z-a")]
        TitleDesc,
        [Display(Name = "Cheap first")]
        CostAsc,
        [Display(Name = "Expensive first")]
        CostDesc,
        [Display(Name = "New first")]
        DateAsc,
        [Display(Name = "Old first")]
        DateDesc
    }
}
