using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.DALModels;

public partial class Country
{
    public int Id { get; set; }
    [Required]
    [StringLength(10, MinimumLength =2)]
    public string Code { get; set; } = null!;

    [Required]
    [StringLength(100, MinimumLength =1)]
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
