using Core.DTOs.Bank;
using Core.DTOs.Region;
using Core.DTOs.User;
using Core.Entities;

namespace Core.Mapper
{
    public class Mapper : AutoMapper.Profile
    {
        public Mapper()
        {
            CreateMap<UserLoginDTO, User>();
            CreateMap<UserDTO, User>();
            CreateMap<UserEditDTO, User>().ReverseMap();
            CreateMap<UserCreateDTO, User>();

            CreateMap<User, UserDTO>()
            .ForMember(dest => dest.BankName, opt => opt.MapFrom(src => src.Bank!.Name))
            .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region!.Name))
            .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.UserName));


            CreateMap<Bank, BankDTO>();
            CreateMap<BankDTO, CreateBankDTO>();
            CreateMap<BankDTO, EditBankDTO>();


            CreateMap<Region, RegionDTO>();
            CreateMap<RegionDTO, CreateRegionDTO>();
            CreateMap<RegionDTO, EditRegionDTO>();
        }
    }
}
