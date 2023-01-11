using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Enums
{
    public enum UserSort
    {
        [Display(Name = "Email a-z")]
        EmailAsc,
        [Display(Name = "Email z-a")]
        EmailDesc,
        [Display(Name = "New first")]
        BirthdayAsc,
        [Display(Name = "Old first")]
        BirthdayDesc,
        [Display(Name = "Name a-z")]
        NameAsc,
        [Display(Name = "Surname a-z")]
        SurnameAsc,
        [Display(Name = "patronymic a-z")]
        PatronymicAsc,
        [Display(Name = "Male first")]
        GenderAsc,
        [Display(Name = "Female first")]
        GenderDesc
    }
}
