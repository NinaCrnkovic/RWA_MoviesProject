using AutoMapper;

namespace MVC_PublicModule.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BL.BLModels.BLUser, ViewModels.VMUser>();
            CreateMap<BL.BLModels.BLCountry, ViewModels.VMCountry>();
            CreateMap<BL.BLModels.BLGenre, ViewModels.VMGenre>();
            CreateMap<BL.BLModels.BLImage, ViewModels.VMImage>();
            CreateMap<BL.BLModels.BLNotification, ViewModels.VMNotification>();
            CreateMap<BL.BLModels.BLTag, ViewModels.VMTag>();
            CreateMap<BL.BLModels.BLVideo, ViewModels.VMVideo>();
            CreateMap<BL.BLModels.BLVideoTag, ViewModels.VMVideoTag>();

            CreateMap<ViewModels.VMUser, BL.BLModels.BLUser>();
            CreateMap<ViewModels.VMCountry, BL.BLModels.BLCountry>();
            CreateMap<ViewModels.VMGenre, BL.BLModels.BLGenre>();
            CreateMap<ViewModels.VMImage, BL.BLModels.BLImage>();
            CreateMap<ViewModels.VMTag, BL.BLModels.BLTag>();
            CreateMap<ViewModels.VMVideo, BL.BLModels.BLVideo>();
            CreateMap<ViewModels.VMVideoTag, BL.BLModels.BLVideoTag>();
        }
    }
}
