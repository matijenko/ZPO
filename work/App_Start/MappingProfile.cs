using AutoMapper;
using work.Controllers.Api;
using work.Models;

namespace work.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<ApplicationUser, UserDto>();
            Mapper.CreateMap<Picture, PictureDto>();
            Mapper.CreateMap<Notification, NotificationDto>();
            Mapper.CreateMap<Category, CategoryDto>();
        }

        protected override void Configure()
        {
            //throw new System.NotImplementedException();
        }
    }
}