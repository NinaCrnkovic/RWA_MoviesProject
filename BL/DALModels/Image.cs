using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BL.DALModels;

public partial class Image
{
    public int Id { get; set; }
    [Required(ErrorMessage = "The Content field is required.")]
    public string Content { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
