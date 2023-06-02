using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.Models;

public partial class Notification
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }
    [Required(ErrorMessage = "The Reciver Email field is required.")]
    public string ReceiverEmail { get; set; } = null!;

    public string? Subject { get; set; }
    [Required(ErrorMessage = "The Body field is required.")]
    public string Body { get; set; } = null!;

    public DateTime? SentAt { get; set; }
}
