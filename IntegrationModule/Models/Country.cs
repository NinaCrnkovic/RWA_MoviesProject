using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IntegrationModule.Models;

public partial class Country
{
    public int Id { get; set; }
    [Required(ErrorMessage = "The Code field is required.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "The Code must be 2 characters long.")]
    public string Code { get; set; }
    [Required]
    [StringLength(256, MinimumLength = 1)]
    public string Name { get; set; }

    //public virtual ICollection<User> Users { get; set; } = new List<User>();
}
