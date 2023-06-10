using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.Models;

public partial class Video
{
    public int Id { get; } = 0;

    public DateTime CreatedAt { get; } = DateTime.Now;
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int GenreId { get; set; }
    [Required]
    public int TotalSeconds { get; set; }
    [Required]
    public string StreamingUrl { get; set; }
    [Required]
    public int ImageId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Image Image { get; set; }

   // public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();

}
