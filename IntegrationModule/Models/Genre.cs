using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.Models;

public partial class Genre
{
    public int Id { get; set; }
    [Required]
    [StringLength(256)]
    public string Name { get; set; }
    [Required]
  
    public string Description { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
