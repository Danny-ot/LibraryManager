using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "All Fields Most Be Inputed")]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName {get; set;}

        [Required(ErrorMessage = "All Fields Most Be Inputed")]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName {get; set;}

        [Required(ErrorMessage = "All Fields Most Be Inputed")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email {get; set;}

        [Required(ErrorMessage = "All Fields Most Be Inputed")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password {get; set;}

        [Required(ErrorMessage = "All Fields Most Be Inputed")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password" , ErrorMessage ="Passwords Do Not Match")]
        public string ConfirmPassword {get; set;}
    }
}