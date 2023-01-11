using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStock.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        [Display(Name = "Registration date")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Address { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime BirthdayDate { get; set; }
        public List<ShopCard> ShopCards { get; set; }
    }
}