using MagicVilla.Models.Models;

namespace MagicVilla.Models.Repository.IRepository
{
	public interface IVillaRepository : IRepository<Villa>
	{
		Task<Villa> UpdateAsync(Villa entity);
	}
}
