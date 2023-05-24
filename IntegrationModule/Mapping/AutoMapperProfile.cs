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
            CreateMap<Models.Genre, BL.BLModels.BLGenre >();
            CreateMap<Models.Tag, BL.BLModels.BLTag>();
            CreateMap<Models.Video, BL.BLModels.BLVideo>();
        }
    }
}
