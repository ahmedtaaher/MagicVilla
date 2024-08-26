using Microsoft.AspNet.Identity.EntityFramework;

namespace MagicVilla.Models.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
	}
}
