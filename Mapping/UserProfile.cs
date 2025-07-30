using AutoMapper;
using KeyRent.Models;      
using KeyRent.ViewModels; 

namespace KeyRent.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>().ReverseMap();
        }
    }
}
