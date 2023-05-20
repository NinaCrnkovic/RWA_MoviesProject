namespace MVC.ViewModels
{
    public class VMGenre
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<VMVideo> Videos { get; set; } = new List<VMVideo>();
    }
}
