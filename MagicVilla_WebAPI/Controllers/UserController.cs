using MagicVilla_WebAPI.DTO;
using MagicVilla_WebAPI.Models;
using MagicVilla_WebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace MagicVilla_WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
        private readonly IUserRepository userRepo;
		protected APIResponse response;
        public UserController(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
			response = new();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO logindto)
        {
			var loginresponse = await userRepo.Login(logindto);
			if(loginresponse.User == null || string.IsNullOrEmpty(loginresponse.Token))
			{
				response.StatusCode = HttpStatusCode.BadRequest;
				response.IsSuccess = false;
				response.ErrorMessages.Add("Username or password is incorrect");
				return BadRequest(response);
			}
			response.StatusCode = HttpStatusCode.OK;
			response.IsSuccess = true;
			response.Result = loginresponse;
			return Ok(response); 
		}
		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterationRequestDTO registerdto)
		{
			bool uniquename = userRepo.IsUniqueUser(registerdto.UserName);
			if(!uniquename)
			{
				response.StatusCode = HttpStatusCode.BadRequest;
				response.IsSuccess = false;
				response.ErrorMessages.Add("Username already exists");
				return BadRequest(response);
			}
			var user = await userRepo.Register(registerdto);
			if(user == null)
			{
				response.StatusCode = HttpStatusCode.BadRequest;
				response.IsSuccess = false;
				response.ErrorMessages.Add("Error while registering");
				return BadRequest(response);
			}
			response.StatusCode = HttpStatusCode.OK;
			response.IsSuccess = true;
			return Ok(response);
		}
	}
}
