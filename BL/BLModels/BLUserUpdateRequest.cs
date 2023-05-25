using System.ComponentModel.DataAnnotations;

namespace BL.BLModels
{
    public class BLUserUpdateRequest
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