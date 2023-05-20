using AutoMapper;

namespace MVC.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BL.BLModels.BLUser, ViewModels.VMUser>();
        }
    }
}
