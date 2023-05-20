using BL.BLModels;
using System;
using System.Collections.Generic;

namespace BL.BLModels;
public partial class BLVideoTag
{
    public int Id { get; set; }

    public int VideoId { get; set; }

    public int TagId { get; set; }

    public virtual BLTag Tag { get; set; } = null!;

    public virtual BLVideo Video { get; set; } = null!;
}
