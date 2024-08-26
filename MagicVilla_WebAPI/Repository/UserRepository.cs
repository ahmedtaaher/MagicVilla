using AutoMapper;
using MagicVilla_WebAPI.DTO;
using MagicVilla_WebAPI.Models;
using MagicVilla_WebAPI.Repository.IRepository;
using MagicVilla_WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_WebAPI.Repository
{
	public class UserRepository : IUserRepository
	{
		private readonly AppDbContext Context;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IMapper mapper;
		private string securityKey;
        public UserRepository(AppDbContext Context, UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration)
        {
            this.Context = Context;
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.mapper = mapper;
			securityKey = configuration.GetValue<string>("APISettings:SecretKey");

		}
        public bool IsUniqueUser(string username)
		{
			var User = Context.ApplicationUsers.FirstOrDefault(c => c.UserName == username);
			if (User == null)
			{
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDTO> Login(LoginRequestDTO loginrequestdto)
		{
			var user = Context.ApplicationUsers.FirstOrDefault(s => s.UserName.ToLower() == loginrequestdto.UserName.ToLower());
			bool isVaild = await userManager.CheckPasswordAsync(user, loginrequestdto.Password);
			if (user == null || isVaild == false)
			{
				return new LoginResponseDTO()
				{
					Token = "",
					User = null,
				};
			}
			var roles = await userManager.GetRolesAsync(user);
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
			};
			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken
				( claims: claims,
				  expires: DateTime.UtcNow.AddDays(7),
				  signingCredentials: credentials
				);

			LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				User = mapper.Map<UserDTO>(user),
			};
			return loginResponseDTO;
		}

		public async Task<UserDTO> Register(RegisterationRequestDTO registerationrequestdto)
		{
			ApplicationUser User = new()
			{
				UserName = registerationrequestdto.UserName,
				Email = registerationrequestdto.UserName,
				NormalizedEmail = registerationrequestdto.UserName.ToUpper(),
				Name = registerationrequestdto.Name,
			};
			try
			{
				var result = await userManager.CreateAsync(User, registerationrequestdto.Password);
				if(result.Succeeded)
				{
					if(!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
					{
						await roleManager.CreateAsync(new IdentityRole("admin"));
						await roleManager.CreateAsync(new IdentityRole("customer"));
					}
					await userManager.AddToRoleAsync(User, "admin");
					var usertoreturn = Context.ApplicationUsers
						.FirstOrDefault(d => d.UserName == registerationrequestdto.UserName);
					return mapper.Map<UserDTO>(usertoreturn);
				}
			}
			catch(Exception e)
			{

			}
			return new UserDTO();
		}
	}
}
