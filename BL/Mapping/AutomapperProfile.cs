﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<DALModels.User, BLModels.BLUser>();
            CreateMap<DALModels.Video, BLModels.BLVideo>();
            CreateMap<DALModels.VideoTag, BLModels.BLVideoTag>();
            CreateMap<DALModels.Tag, BLModels.BLTag>();
            CreateMap<DALModels.Genre, BLModels.BLGenre>();
            CreateMap<DALModels.Country, BLModels.BLCountry>();
            CreateMap<DALModels.Image, BLModels.BLImage>();
            CreateMap<DALModels.Notification, BLModels.BLNotification>();

            


            CreateMap<BLModels.BLGenre, DALModels.Genre>();
            CreateMap<BLModels.BLTag, DALModels.Tag>();
            CreateMap<BLModels.BLVideo, DALModels.Video>();
            CreateMap<BLModels.BLUser, DALModels.User>();
            CreateMap<BLModels.BLNotification, DALModels.Notification>();
            CreateMap<BLModels.BLVideoTag, DALModels.VideoTag>();
            CreateMap<BLModels.BLCountry, DALModels.Country>();
            CreateMap<BLModels.BLImage, DALModels.Image>();


            CreateMap<BL.DALModels.Tag, BL.BLModels.BLVideoTag>();

        }
    }
}
