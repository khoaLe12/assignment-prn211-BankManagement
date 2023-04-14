using BankManagement.Entities;
using BankManagement.DataAccess;

namespace BankManagement.Repository;

public class AccountRepository : IAccountRepository
{
    public async Task<Account?> FindAccountByID(int accountId) => await AccountDAO.Instance.GetAccountByID(accountId);

    public async Task<IEnumerable<Account>> GetAllAccountOfaBank(int bankId) => await AccountDAO.Instance.GetAccountListOfaBank(bankId);

    public async Task InsertAccount(Account account) => await AccountDAO.Instance.AddNewAccount(account);
}
