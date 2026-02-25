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
            
            CreateMap<MemberShip, MembershipViewModel>()
                .ForMember(des => des.MemberName, options => options.MapFrom(src => src.Member.Name))
                .ForMember(des => des.StartDate, options => options.MapFrom(src => src.CreatedAt))
                .ForMember(des => des.PlanName, options => options.MapFrom(src => src.Plan.Name))
                .ForMember(des => des.Price, options => options.MapFrom(src => src.Plan.Price))
                .ForMember(des => des.RemainingDays, options => options.MapFrom(src => (src.EndDate - DateTime.Now).Days));

            CreateMap<Booking, BookingViewModel>()
                .ForMember(des => des.MemberName, options => options.MapFrom(src => src.Member.Name))
                .ForMember(des => des.TrainerName, options => options.MapFrom(src => src.Session.Trainer.Name))
                .ForMember(des => des.CategoryName, options => options.MapFrom(src => src.Session.Category.CategoryName))
                .ForMember(des => des.StartDate, options => options.MapFrom(src => src.Session.StartDate))
                .ForMember(des => des.EndDate, options => options.MapFrom(src => src.Session.EndDate));
        }
    }
}
