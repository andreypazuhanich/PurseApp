using System.ComponentModel.DataAnnotations;

namespace PurseApp.Models.Dto
{
    public class AuthenticateRequest
    {
        [Required]
        public string UserName { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        [Display(Name = "Запомнить")]
        public bool IsRemember { get; set; }
    }
}