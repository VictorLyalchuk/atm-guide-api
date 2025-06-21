using AutoMapper;
using Core.DTOs.Bank;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;

namespace Core.Services
{
    internal class BankService : IBankService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Bank> _bankRepository;
        public BankService(IMapper mapper, IRepository<Bank> bankRepository)
        {
            _mapper = mapper;
            _bankRepository = bankRepository;
        }
        public async Task CreateAsync(CreateBankDTO createBankDTO)
        {
            try
            {
                var bank = _mapper.Map<Bank>(createBankDTO);
                await _bankRepository.InsertAsync(bank);
                await _bankRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                var errors = new List<string>();

                if (ex.Message.Contains("duplicate") || ex.Message.Contains("унікальн"))
                {
                    errors.Add("Банк з такою назвою вже існує.");
                }
                else
                {
                    errors.Add("Помилка при збереженні: " + ex.Message);
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            var bank = await _bankRepository.GetByIDAsync(id);
            if (bank == null)
            await _bankRepository.DeleteAsync(bank);
            await _bankRepository.SaveAsync();
        }

        public async Task EditAsync(EditBankDTO editBankDTO)
        {
            try
            {
                var bank = _mapper.Map<Bank>(editBankDTO);
                await _bankRepository.UpdateAsync(bank);
                await _bankRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                var errors = new List<string>();

                if (ex.Message.Contains("duplicate") || ex.Message.Contains("унікальн"))
                {
                    errors.Add("Банк з такою назвою вже існує.");
                }
                else
                {
                    errors.Add("Помилка при збереженні: " + ex.Message);
                }
            }
        }

        public async Task<List<BankDTO>> GetAllAsync()
        {
            var banks = await _bankRepository.GetListBySpec(new BankSpecification.BankAll());
            return _mapper.Map<List<BankDTO>>(banks);

        }

        public async Task<BankDTO> GetBankByIDAsync(int id)
        {
            var bank = await _bankRepository.GetByIDAsync(id);
            return _mapper.Map<BankDTO>(bank);
        }

        public async Task<List<BankDTO>> BankByPageAsync(int page)
        {
            var banks = await _bankRepository.GetListBySpec(new BankSpecification.BankByPage(page));
            return _mapper.Map<List<BankDTO>>(banks);
        }

        public async Task<int> BankQuantityAsync()
        {
            var banks = await _bankRepository.GetAsync();
            return banks.Count();
        }
    }
}
