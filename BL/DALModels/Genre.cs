using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.DALModels;

public partial class Genre
{
    public int Id { get; set; }
 
    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
