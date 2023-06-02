using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class VMCountry
    {
        public int Id { get; set; }
        [Required]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "The Code must be 2 characters long.")]
        public string Code { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        public virtual ICollection<VMUser> Users { get; set; } = new List<VMUser>();
    }
}
