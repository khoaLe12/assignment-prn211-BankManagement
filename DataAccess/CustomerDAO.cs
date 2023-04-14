using BankManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.DataAccess;
public class CustomerDAO
{
    //Singleton pattern
    private static CustomerDAO? instance = null;
    private static readonly object instanceLock = new object();

    public static CustomerDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                if (instance == null)
                {
                    instance = new CustomerDAO();
                }
                return instance;
            }
        }
    }

    //Get List Of Customers
    public async Task<IEnumerable<Customer>> GetAllCustomers()
    {
        var customers = new List<Customer>();
        try
        {
            using var context = new BankContextFactory().CreateDbContext();
            customers = await context.Customers
                .AsNoTracking()
                .Include(ct => ct.Bank)
                .Include(ct => ct.Accounts)
                .Include(nameof(Customer.Accounts) + "." + nameof(Account.Transactions))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return customers;
    }


    //Get A Customer By ID
    public async Task<Customer?> GetCustomerByID(int cusID)
    {
        Customer? customer = null;
        try
        {
            using var context = new BankContextFactory().CreateDbContext();
            customer = await context.Customers
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == cusID);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return customer;
    }


    //Add New Customer If Customer Does Not Exist In DB
    public async Task AddNewCustomer(Customer customer)
    {
        try
        {
            Customer? _customer = await GetCustomerByID(customer.Id);
            if (_customer == null)
            {
                using var context = new BankContextFactory().CreateDbContext();
                context.Attach(customer.Bank);
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}