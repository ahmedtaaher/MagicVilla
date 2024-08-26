using MagicVilla_WebAPI.DTO;

namespace MagicVilla_WebAPI.Repository.IRepository
{
	public interface IUserRepository
	{
		bool IsUniqueUser(string username);
		Task<LoginResponseDTO> Login(LoginRequestDTO loginrequestdto);
		Task<UserDTO> Register(RegisterationRequestDTO registerationrequestdto);

	}
}
