namespace MVC.ViewModels
{
    public class VMCountry
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public virtual ICollection<VMUser> Users { get; set; } = new List<VMUser>();
    }
}
