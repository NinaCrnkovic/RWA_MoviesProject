using System;
using System.Collections.Generic;

namespace BL.BLModels;

public partial class BLNotification
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ReceiverEmail { get; set; } = null!;

    public string? Subject { get; set; }

    public string Body { get; set; } = null!;

    public DateTime? SentAt { get; set; }
}
