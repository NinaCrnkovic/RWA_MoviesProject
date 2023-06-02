using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BL.DALModels;

public partial class Country
{
    public int Id { get; set; }
 
    public string? Code { get; set; } 


    public string? Name { get; set; } 

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
