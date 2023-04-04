using AutoMapper;
using WebLearning.Contract.Dtos.Login;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.LoginMapping
{
    public class LoginMappingPofile : Profile
    {
        public LoginMappingPofile()
        {

            CreateMap<LoginDto, Account>()
                        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                        .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

        }


    }
}
