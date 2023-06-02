using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class VMNotification
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "The Reciver Email field is required.")]
        [StringLength(256)]
        public string ReceiverEmail { get; set; } = null!;

        public string Subject { get; set; }
        [Required(ErrorMessage = "The Body field is required.")]
        [StringLength(1023)]
        public string Body { get; set; } = null!;

        public DateTime? SentAt { get; set; }
    }

}
