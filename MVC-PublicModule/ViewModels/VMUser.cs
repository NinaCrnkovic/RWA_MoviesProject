using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_PublicModule.ViewModels
{
    public class VMUser
    {
        public int Id { get; set; }

        [DisplayName("Created")]
   
        public DateTime CreatedAt { get; set; }

        [DisplayName("Deleted")]
      
        public DateTime? DeletedAt { get; set; }

        [DisplayName("User name")]
        [Required]
        public string Username { get; set; } = null!;
        [DisplayName("First name:")]
        [Required]
        public string FirstName { get; set; } = null!;
        [DisplayName("Last name:")]
        [Required]
        public string LastName { get; set; } = null!;
        [DisplayName("E-mail")]
        [Required]
        public string Email { get; set; } = null!;


        [Required]
        public string Phone { get; set; }

        public bool IsConfirmed { get; set; }

        public string? SecurityToken { get; set; }

        [DisplayName("Country")]
        [Required]
        public int CountryOfResidenceId { get; set; }
       
        public virtual VMCountry CountryOfResidence { get; set; } = null!;
    }
}
