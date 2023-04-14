using BankManagement.Entities;

namespace BankManagement.Repository;

public interface IAccountRepository
{
    Task InsertAccount(Account account);
    Task<Account?> FindAccountByID(int accountId);
    Task<IEnumerable<Account>> GetAllAccountOfaBank(int bankId);
}
