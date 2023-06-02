using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_PublicModule.ViewModels
{
    public class VMRegister
    {
        [DisplayName("User name")]
        [Required]
        public string Username { get; set; }
        [DisplayName("E-mail")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DisplayName("Confirm e-mail")]
        [Compare("Email")]
        [Required]
        public string Email2 { get; set; }
        [DisplayName("First name")]
        [Required]
        public string FirstName { get; set; }
        [DisplayName("Last name")]
        [Required]
        public string LastName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
        [DisplayName("Repeat password")]
        [Compare("Password")]
        [Required]
        public string Password2 { get; set; }
        [DisplayName("Country")]
        [Required]
        public int CountryOfResidenceId { get; set; }
    }
}
