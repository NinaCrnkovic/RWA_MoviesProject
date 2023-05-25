using System;
using System.Collections.Generic;

namespace IntegrationModule.Models;

public partial class Video
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; }

    public int GenreId { get; set; }

    public int TotalSeconds { get; set; }

    public string StreamingUrl { get; set; }

    public int ImageId { get; set; }

    
}
