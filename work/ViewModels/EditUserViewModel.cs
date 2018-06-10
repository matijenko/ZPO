using System.ComponentModel.DataAnnotations;

namespace work.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Wrong form of Email.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 9)]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }
}