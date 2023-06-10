using System.ComponentModel.DataAnnotations;

namespace MVC_PublicModule.ViewModels
{
    public class VMGenre
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(256)]
        public string Name { get; set; }
        [StringLength(1023)]
        public string? Description { get; set; }


   
    }
}
