using BankManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.DataAccess;
public class AccountDAO{
    private static AccountDAO? instance = null;
    private static readonly object instanceLock = new object();

    public static AccountDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new AccountDAO();
                }
                return instance;
            }
        }
    }

    //Get List Of Account
    public async Task<IEnumerable<Account>> GetAccountListOfaBank(int bankId)
    {
        try
        {
            using var context = new BankContextFactory().CreateDbContext();
            return await context.Accounts
                .Where(a => a.Customer.Bank.Id == bankId)
                .Include(a => a.Transactions)
                .Include(a => a.Customer)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //Get A Account By ID
    public async Task<Account?> GetAccountByID(int accountId)
    {
        try
        {
            using var context = new BankContextFactory().CreateDbContext();
            return await context.Accounts
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == accountId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    //Add New Account If Account Does Not Exist In DB
    public async Task AddNewAccount(Account account)
    {
        try
        {
            Account? _account = await GetAccountByID(account.Id);
            if (_account == null)
            {
                using var context = new BankContextFactory().CreateDbContext();
                context.Attach(account.Customer);
                await context.Accounts.AddAsync(account);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}