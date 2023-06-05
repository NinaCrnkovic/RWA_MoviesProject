using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class VMUser
    {
        public int Id { get; set; }

        [DisplayName("Created")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Deleted")]
        public DateTime? DeletedAt { get; set; }

        [DisplayName("User name")]
        [Required(ErrorMessage = "The field is required.")]
        [StringLength(256)]
        public string Username { get; set; } = null!;
        [DisplayName("First name:")]
        [Required(ErrorMessage = "The field is required.")]
        [StringLength(256)]
        public string FirstName { get; set; } = null!;
        [DisplayName("Last name:")]
        [Required(ErrorMessage = "The field is required.")]
        [StringLength(256)]
        public string LastName { get; set; } = null!;
        [Required(ErrorMessage = "The field is required.")]
        [StringLength(256)]
        public string Email { get; set; } = null!;

        public string PwdHash { get; set; } = null!;

        public string PwdSalt { get; set; } = null!;

        public string Phone { get; set; }

        public bool IsConfirmed { get; set; }

        public string SecurityToken { get; set; }

        [DisplayName("Country id")]
        public int CountryOfResidenceId { get; set; }
        [DisplayName("Country")]
        public virtual VMCountry CountryOfResidence { get; set; }
    }
}
