using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.Models
{
    public class UserUpdateRequest
    {
        [Required]

        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }



        public int CountryOfResidenceId { get; set; }
    }
}