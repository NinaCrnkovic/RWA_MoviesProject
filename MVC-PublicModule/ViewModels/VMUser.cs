using System.ComponentModel;

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
        public string Username { get; set; } = null!;
        [DisplayName("First name:")]
        public string FirstName { get; set; } = null!;
        [DisplayName("Last name:")]
        public string LastName { get; set; } = null!;
        [DisplayName("E-mail")]
        public string Email { get; set; } = null!;



        public string Phone { get; set; }

        public bool IsConfirmed { get; set; }

        public string? SecurityToken { get; set; }

        [DisplayName("Country")]
        public int CountryOfResidenceId { get; set; }
       
       // public virtual VMCountry CountryOfResidence { get; set; } = null!;
    }
}
