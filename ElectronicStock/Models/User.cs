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
        [Required(ErrorMessage = "Empty field")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Invalid length, it should be between 5 and 20 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Empty field")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Invalid length, it should be between 5 and 20 characters")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Empty field")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Invalid length, it should be between 5 and 20 characters")]
        public string Patronymic { get; set; }
        [Display(Name = "Registration date")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Empty field")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Empty field")]
        public string Address { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime BirthdayDate { get; set; }
        public List<ShopCard> ShopCards { get; set; }
    }
}