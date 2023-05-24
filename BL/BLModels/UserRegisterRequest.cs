using System.ComponentModel.DataAnnotations;

namespace BL.BLModels
{
    public class UserRegisterRequest
    {
        [Required, StringLength(50, MinimumLength = 6)]
        public string Username { get; set; }
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


        //[Required]
        //public string Role { get; set; }

        public int CountryOfResidenceId { get; set; }
    }
}