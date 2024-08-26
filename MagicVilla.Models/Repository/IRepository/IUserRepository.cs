using MagicVilla.Models.DTO;

namespace MagicVilla.Models.Repository.IRepository
{
	public interface IUserRepository
	{
		bool IsUniqueUser(string username);
		Task<LoginResponseDTO> Login(LoginRequestDTO loginrequestdto);
		Task<UserDTO> Register(RegisterationRequestDTO registerationrequestdto);

	}
}
