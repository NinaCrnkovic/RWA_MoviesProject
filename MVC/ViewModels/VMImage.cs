namespace MVC.ViewModels
{
    public class VMImage
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public virtual ICollection<VMVideo> Videos { get; set; } = new List<VMVideo>();
    }

}
