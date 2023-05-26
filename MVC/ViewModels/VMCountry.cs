using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class VMCountry
    {
        public int Id { get; set; }
        [MaxLength(2)]
        public string Code { get; set; }
        public string Name { get; set; } 

        //public virtual ICollection<VMUser> Users { get; set; } = new List<VMUser>();
    }
}
