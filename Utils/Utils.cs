using BankManagement.Entities;
using System.Globalization;

namespace BankManagement.Utils;

public class Utils
{
    public int ShowOptions(String[] options)
    {
        int choice = -1;
        Console.WriteLine("----------------OPTIONS-----------------");
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine(options[i]);
        }
        Console.Write("\nYour Choice: ");
        if(int.TryParse(Console.ReadLine(), out choice) == false)
            choice = -1;
        return choice;
    }

    public async Task AddData()
    {
        var factory = new BankContextFactory();
        using var context = factory.CreateDbContext();
        using var transaction = await context.Database.BeginTransactionAsync();
        //Add Banks
        Bank Techcombank, Sacombank;
        await context.AddRangeAsync(new[]
        {
            Techcombank = new Bank(){BankName = "Techcombank", BankAddress = "301 Nguyễn Sơn, Phú Thạnh, Tân Phú, Thành phố Hồ Chí Minh"},
            Sacombank = new Bank(){BankName = "Sacombank", BankAddress = "324 Nguyễn Sơn, Phú Thọ Hoà, Tân Phú, Thành phố Hồ Chí Minh"},
        });
        await context.SaveChangesAsync();

        //Add Customer
        Customer a, b, c;
        await context.AddRangeAsync(new[]
        {
            a = new Customer(){CustomerName = "Hoa", CustomerAddress = "Q.1", Bank = Techcombank},
            b = new Customer(){CustomerName = "Khanh", CustomerAddress = "Q.2", Bank = Techcombank},
            c = new Customer(){CustomerName = "Teo", CustomerAddress = "Q.3", Bank = Sacombank},
        });
        await context.SaveChangesAsync();

        //Add Accounts
        Account d, e, f, g;
        await context.AddRangeAsync(new[]
        {
            d = new Account(){Balance = 1000.45M, Customer = a},
            e = new Account(){Balance = 2000, Customer = a},
            f = new Account(){Balance = 1234.56M, Customer = b},
            g = new Account(){Balance = 301, Customer = c},
        } );
        await context.SaveChangesAsync();

        //Add Transactions
        Transaction h, k;
        await context.AddRangeAsync(new[]
        {
            h = new Transaction(){Money = 200, AccountFrom = d, AccountTo = f , TransactionDate = DateTime.ParseExact("2023-02-05","yyyy-MM-dd",CultureInfo.InvariantCulture)},
            k = new Transaction(){Money = 400, AccountFrom = g, AccountTo = e , TransactionDate = DateTime.ParseExact("2023-04-10","yyyy-MM-dd",CultureInfo.InvariantCulture)},
        });
        await context.SaveChangesAsync();

        await transaction.CommitAsync();
    }
}
