using Microsoft.AspNetCore.Identity;

namespace MagicVilla_WebAPI.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
	}
}
