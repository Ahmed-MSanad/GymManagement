using AutoMapper;
using GymManagementBLL.ViewModels;
using GymManagementDAL.Entities;

namespace GymManagementBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
        }

        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(des => des.CategoryName, options => options.MapFrom(src => src.Category.CategoryName))
                .ForMember(des => des.TrainerName, options => options.MapFrom(src => src.Trainer.Name))
                .ForMember(des => des.AvailableSlots, options => options.Ignore());
            
            CreateMap<SessionCreateViewModel, Session>();

            CreateMap<SessionUpdateViewModel, Session>().ReverseMap();

            CreateMap<Category, CategorySelectViewModel>();

            CreateMap<Trainer, TrainerSelectViewModel>();
        }
    }
}
