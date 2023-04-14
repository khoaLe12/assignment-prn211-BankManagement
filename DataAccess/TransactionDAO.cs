using BankManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.DataAccess;
public class TransactionDAO
{
    private static TransactionDAO? instance = null;
    private static readonly object instanceLock = new object();

    public static TransactionDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new TransactionDAO();
                }
                return instance;
            }
        }
    }

    //Get a Transaction List
    public async Task<IEnumerable<Transaction>> GetTransactionList()
    {
        var factory = new BankContextFactory();
        using var context = factory.CreateDbContext();
        var transactions = new List<Transaction>();
        try
        {
            transactions = await context.Transactions.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return transactions;
    }

    //Get A Transaction By ID
    public async Task<Transaction?> GetTransactionByID(int tranId)
    {
        try
        {
            using var context = new BankContextFactory().CreateDbContext();
            return await context.Transactions
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == tranId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    //Add New Transaction If Transaction Does Not Exist In DB
    public async Task AddNewTransaction(Transaction transaction)
    {
        try
        {
            Transaction? _transaction = await GetTransactionByID(transaction.Id);
            if (_transaction == null)
            {
                using var context = new BankContextFactory().CreateDbContext();
                using var trans = await context.Database.BeginTransactionAsync();

                Account? account1 = await context.Accounts.SingleOrDefaultAsync(a => a.Id==transaction.AccountFrom.Id);
                Account? account2 = await context.Accounts.SingleOrDefaultAsync(a => a.Id == transaction.AccountTo.Id);

                if(account1 != null && account2 != null)
                {
                    //Add new transaction
                    transaction.AccountFrom = account1;
                    transaction.AccountTo = account2;
                    await context.Transactions.AddAsync(transaction);
                    await context.SaveChangesAsync();
                    //Make a transaction
                    account1.Balance -= transaction.Money;
                    account2.Balance += transaction.Money;
                    await context.SaveChangesAsync();
                }

                await trans.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}