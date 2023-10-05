using MagicVilla_MVC.Models.DTOs.Villa_Number_DTOs;
using MagicVilla_MVC.Models.VM;

namespace MagicVilla_MVC.Services.IServices
{
	public interface IVillaNumberService
	{
		Task<T> GetAllAsync<T>();
		Task<T> GetAsync<T>(int id);
		Task<T> CreateAsync<T>(CreateNumberDTO dto);
		Task<T> UpdateAsync<T>(UpdateNumberDTO dto);
		Task<T> DeleteAsync<T>(int id);
    }
}
