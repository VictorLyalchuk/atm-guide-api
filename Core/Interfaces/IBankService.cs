using Core.DTOs.Bank;

namespace Core.Interfaces
{
    public interface IBankService
    {
        Task<List<BankDTO>> GetAllAsync();
        Task<List<BankDTO>> BankByPageAsync(int page);
        Task<BankDTO> GetBankByIDAsync(int id);
        Task CreateAsync(CreateBankDTO createBankDTO);
        Task DeleteAsync(int id);
        Task EditAsync(EditBankDTO editBankDTO);
        Task<int> BankQuantityAsync();
    }
}
