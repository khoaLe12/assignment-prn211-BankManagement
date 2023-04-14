using BankManagement.Entities;

namespace BankManagement.Repository;

public interface IBankRepository
{
    Task GetAllBank();
    Task InsertBank(Bank bank);
    Task<Bank?> FindBankByID(int bankId);
}
