using AutoMapper;
using WebLearning.Contract.Dtos.Account;
using WebLearning.Domain.Entites;

namespace WebLearning.Application.Mapping.AccountMappinng
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {

            CreateMap<AccountDetail, AccountDetailDto>();
            CreateMap<CreateAccountDetailDto, AccountDetail>().ForMember(x => x.Id, opt => opt.Ignore())
                                                            .ForMember(x => x.AccountId, opt => opt.Ignore());


            CreateMap<AccountDetailDto, AccountDetail>().ForMember(x => x.Id, opt => opt.Ignore())
                                                            .ForMember(x => x.AccountId, opt => opt.Ignore());

            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                    .ForMember(dest => dest.PasswordHased, opt => opt.MapFrom(src => src.PasswordHased))
                    .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                    .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                    .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
                    .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))

                    .ForPath(dest => dest.roleDto.Id, opt => opt.MapFrom(src => src.RoleId))
                    .ForPath(dest => dest.roleDto.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                    .ForPath(dest => dest.roleDto.Description, opt => opt.MapFrom(src => src.Role.Description))

                    .ForPath(dest => dest.accountDetailDto.Id, opt => opt.MapFrom(src => src.AccountDetail.Id))
                    .ForPath(dest => dest.accountDetailDto.AccoundId, opt => opt.MapFrom(src => src.AccountDetail.AccountId))
                    .ForPath(dest => dest.accountDetailDto.FullName, opt => opt.MapFrom(src => src.AccountDetail.FullName))
                    .ForPath(dest => dest.accountDetailDto.Phone, opt => opt.MapFrom(src => src.AccountDetail.Phone))
                    .ForPath(dest => dest.accountDetailDto.Address, opt => opt.MapFrom(src => src.AccountDetail.Address))
                    .ForPath(dest => dest.AuthorizeRole, opt => opt.MapFrom(src => src.AuthorizeRole))
                    .ForPath(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar));

            CreateMap<CreateAccountDto, Account>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
                                                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                                                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                                                .ForMember(dest => dest.PasswordHased, opt => opt.MapFrom(src => src.PasswordHased))
                                                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                                                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                                                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                                                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                                                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
                                                .ForPath(dest => dest.AccountDetail.Id, opt => opt.MapFrom(src => src.AccountDetailId))
                                                .ForPath(dest => dest.AccountDetail.FullName, opt => opt.MapFrom(src => src.FullName))
                                                .ForPath(dest => dest.AccountDetail.Address, opt => opt.MapFrom(src => src.Address))
                                                .ForPath(dest => dest.AccountDetail.Phone, opt => opt.MapFrom(src => src.Phone))
                                                .ForPath(dest => dest.AuthorizeRole, opt => opt.MapFrom(src => src.AuthorizeRole));

            CreateMap<UpdateAccountDto, Account>()
                .ForMember(x => x.Id, opt => opt.Ignore())

                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.PasswordHased, opt => opt.MapFrom(src => src.PasswordHased))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => src.LastLogin))
                .ForPath(dest => dest.AccountDetail.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForPath(dest => dest.AccountDetail.Address, opt => opt.MapFrom(src => src.Address))
                .ForPath(dest => dest.AccountDetail.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForPath(dest => dest.AuthorizeRole, opt => opt.MapFrom(src => src.AuthorizeRole));




        }
    }
}
