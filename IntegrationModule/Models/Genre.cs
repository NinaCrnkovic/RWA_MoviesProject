using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.Models;

public partial class Genre
{
    public int Id { get; set; }
    [Required(ErrorMessage = "The Name field is required.")]
    [StringLength(256)]
    public string Name { get; set; }
    [StringLength(1023)]
    public string Description { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
