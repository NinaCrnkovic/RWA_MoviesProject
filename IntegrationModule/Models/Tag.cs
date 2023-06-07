using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.Models;

public partial class Tag
{
    public int Id { get; set; }
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = null!;


    public virtual ICollection<VideoTag> VideoTags { get; set; } = new List<VideoTag>();
}
