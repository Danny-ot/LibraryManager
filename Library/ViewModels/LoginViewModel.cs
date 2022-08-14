using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "All inputs must be entered")]
        public string Email {get; set;}

        [Required(ErrorMessage = "All inputs must be entered")]
        public string Password {get; set;}
    }
}