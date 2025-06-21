using Core.DTOs.Region;

namespace Core.Interfaces
{
    public interface IRegionService
    {
        Task<List<RegionDTO>> GetAllAsync();
        Task<List<RegionDTO>> RegionByPageAsync(int page);
        Task<RegionDTO> GetRegionByIDAsync(int id);
        Task CreateAsync(CreateRegionDTO createRegionDTO);
        Task DeleteAsync(int id);
        Task EditAsync(EditRegionDTO editRegionDTO);
        Task<int> RegionQuantityAsync();
    }
}
