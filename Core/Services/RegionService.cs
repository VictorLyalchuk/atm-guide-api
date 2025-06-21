using AutoMapper;
using Core.DTOs.Region;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;

namespace Core.Services
{
    public class RegionService : IRegionService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Region> _regionRepository;
        public RegionService(IMapper mapper, IRepository<Region> regionRepository)
        {
            _mapper = mapper;
            _regionRepository = regionRepository;
        }
        public async Task CreateAsync(CreateRegionDTO createRegionDTO)
        {
            try
            {
                var region = _mapper.Map<Region>(createRegionDTO);
                await _regionRepository.InsertAsync(region);
                await _regionRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                var errors = new List<string>();

                if (ex.Message.Contains("duplicate") || ex.Message.Contains("унікальн"))
                {
                    errors.Add("Регіон з такою назвою вже існує.");
                }
                else
                {
                    errors.Add("Помилка при збереженні: " + ex.Message);
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            var bank = await _regionRepository.GetByIDAsync(id);
            await _regionRepository.DeleteAsync(bank);
            await _regionRepository.SaveAsync();
        }

        public async Task EditAsync(EditRegionDTO editRegionDTO)
        {
            try
            {
                var bank = _mapper.Map<Region>(editRegionDTO);
                await _regionRepository.UpdateAsync(bank);
                await _regionRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                var errors = new List<string>();

                if (ex.Message.Contains("duplicate") || ex.Message.Contains("унікальн"))
                {
                    errors.Add("Регіон з такою назвою вже існує.");
                }
                else
                {
                    errors.Add("Помилка при збереженні: " + ex.Message);
                }
            }
        }

        public async Task<List<RegionDTO>> GetAllAsync()
        {
            var regions = await _regionRepository.GetListBySpec(new RegionSpecification.RegionAll());
            return _mapper.Map<List<RegionDTO>>(regions);
        }

        public async Task<RegionDTO> GetRegionByIDAsync(int id)
        {
            var region = await _regionRepository.GetByIDAsync(id);
            return  _mapper.Map<RegionDTO>(region);
        }

        public async Task<List<RegionDTO>> RegionByPageAsync(int page)
        {
            var regions = await _regionRepository.GetListBySpec(new RegionSpecification.RegionByPage(page));
            return _mapper.Map<List<RegionDTO>>(regions);
        }

        public async Task<int> RegionQuantityAsync()
        {
            var regions = await _regionRepository.GetAsync();
            return regions.Count();
        }
    }
}
