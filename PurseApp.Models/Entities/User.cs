using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PurseApp.Models
{
    public class User : IdentityUser
    {
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        [Display(Name = "Запомнить")]
        public bool IsRemember { get; set; }
    }
}