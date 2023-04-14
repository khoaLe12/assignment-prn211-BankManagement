using BankManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.DataAccess;

public class BankDAO
{
    private static BankDAO? instance = null;
    private static readonly object instanceLock = new object();

    public static BankDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new BankDAO();
                }
                return instance;
            }
        }
    }

    //Get List Of Bank (Array)
    public async Task<IEnumerable<Bank>> GetBankList()
    {
        try
        {
            using var context = new BankContextFactory().CreateDbContext();
            return await context.Banks
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    //Get A Bank By ID
    public async Task<Bank?> GetBankByID(int bankID)
    {
        Bank? bank = null;
        try
        {
            using var context = new BankContextFactory().CreateDbContext();
            bank = await context.Banks
                .AsNoTracking()
                .SingleOrDefaultAsync(b => b.Id == bankID);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return bank;
    }

    //Add New Bank
    public async Task AddNewBank(Bank bank)
    {
        try
        {
            Bank? _bank = await GetBankByID(bank.Id);
            if(_bank == null)
            {
                using var context = new BankContextFactory().CreateDbContext();
                await context.Banks.AddAsync(bank);
                await context.SaveChangesAsync();
            }
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
