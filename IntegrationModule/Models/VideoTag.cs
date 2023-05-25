using System;
using System.Collections.Generic;

namespace IntegrationModule.Models;

public partial class VideoTag
{
    public int Id { get; set; }

    public int VideoId { get; set; }

    public int TagId { get; set; }


}
