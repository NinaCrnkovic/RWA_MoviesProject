using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class VMTag
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(256)]
        public string Name { get; set; } = null!;


        public virtual ICollection<VMVideoTag> VideoTags { get; set; } = new List<VMVideoTag>();
    }
}
