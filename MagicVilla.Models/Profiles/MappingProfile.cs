using AutoMapper;
using MagicVilla.Models.DTO;
using MagicVilla.Models.Models;

namespace MagicVilla.Models.Profiles
{
	public class MappingProfile : Profile
	{
		public MappingProfile() 
		{
			CreateMap<Villa, VillaDTO>().ReverseMap();
			CreateMap<Villa, VillaCreateDTO>().ReverseMap();
			CreateMap<Villa, VillaUpdateDTO>().ReverseMap();
			CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
			CreateMap<VillaNumber, VillaNumberCreateDTO>().ReverseMap();
			CreateMap<VillaNumber, VillaNumberUpdateDTO>().ReverseMap();
			CreateMap<ApplicationUser, UserDTO>().ReverseMap();
		}
	}
}
