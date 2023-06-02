﻿using System.ComponentModel.DataAnnotations;

namespace MVC_PublicModule.ViewModels
{
    public class VMTag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(256)]
        public string Name { get; set; } = null!;

        public virtual ICollection<VMVideoTag> VideoTags { get; set; } = new List<VMVideoTag>();
    }
}
