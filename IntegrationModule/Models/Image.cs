using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.Models;

public partial class Image
{
    public int Id { get; set; }
    [Required(ErrorMessage = "The Content field is required.")]
    public string Content { get; set; } = null!;

    
}
