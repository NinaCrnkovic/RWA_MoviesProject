using AutoMapper;

namespace IntegrationModule.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BL.BLModels.BLUser, Models.User>();
            CreateMap<BL.BLModels.BLCountry, Models.Country>();
            CreateMap<BL.BLModels.BLGenre, Models.Genre>();
            CreateMap<BL.BLModels.BLImage, Models.Image>();
            CreateMap<BL.BLModels.BLNotification, Models.Notification>();
            CreateMap<BL.BLModels.BLTag, Models.Tag>();
            CreateMap<BL.BLModels.BLVideo, Models.Video>();
            CreateMap<BL.BLModels.BLVideoTag, Models.VideoTag>();


     



            CreateMap<Models.User, BL.BLModels.BLUser>();
            CreateMap<Models.Country, BL.BLModels.BLCountry>();
            CreateMap<Models.Genre, BL.BLModels.BLGenre >();
            CreateMap<Models.Image, BL.BLModels.BLImage>();
            CreateMap<Models.Notification, BL.BLModels.BLNotification>();
            CreateMap<Models.Tag, BL.BLModels.BLTag>();
            CreateMap<Models.Video, BL.BLModels.BLVideo>();
            CreateMap<Models.VideoTag, BL.BLModels.BLVideoTag>();



        }
    }
}
