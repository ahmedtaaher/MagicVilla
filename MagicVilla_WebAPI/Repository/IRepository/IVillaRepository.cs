using MagicVilla_WebAPI.Models;

namespace MagicVilla_WebAPI.Repository.IRepository
{
	public interface IVillaRepository : IRepository<Villa>
	{
		Task<Villa> UpdateAsync(Villa entity);
	}
}
