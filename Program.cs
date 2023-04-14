using BankManagement.Entities;
using BankManagement.Repository;
using BankManagement.Utils;

public class Program
{
    static IBankRepository bankRepository = new BankRepository();
    static ICustomerRepository customerRepository = new CustomerRepository();
    static IAccountRepository accountRepository = new AccountRepository();
    static ITransactionRepository transactionRepository = new TransactionRepository();
    public static async Task Main(string[] args)
    {
        Utils utils = new();

        string[] options1 = new string[] {"0.Exit", 
            "1.Create new bank", "2.Show all bank", "3.Choose bank"};

        bool condition = true;

        //Add Data
        //await utils.AddData();

        Bank? bank;

        while (condition)
        {
            switch (utils.ShowOptions(options1))
            {
                case 0:
                    condition = false;
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("------Creating New Bank------");
                    bank = await bankRepository.FindBankByID(await CreateNewBank());
                    if (bank != null)
                    {
                        Console.Clear();
                        Console.WriteLine("------Created New Bank Succesfully------");
                        await BankManagement(bank);
                    }
                    else
                    {
                        Console.WriteLine("------Created New Bank Fail!!!------");
                    }
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("------All Banks------");
                    await bankRepository.GetAllBank();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("------All Banks------");
                    await bankRepository.GetAllBank();
                    Console.Write("Input Bank Id you want to choose: ");
                    int id = -1;
                    if(int.TryParse(Console.ReadLine(), out id))
                    {
                        bank = await bankRepository.FindBankByID(id);
                        if (bank != null)
                        {
                            Console.Clear();
                            await BankManagement(bank);
                        }
                        else
                        {
                            Console.WriteLine("No Bank found!!!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Bank Id!!!");
                    }
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input, please input again!!!");
                    break;
            }
        }
    }

    public static async Task<int> CreateNewBank()
    {
        string? _bankName;
        string? _bankAddress;
        try
        {
            do
            {
                Console.Write("Input Bank Name: ");
                _bankName = Console.ReadLine();
            } while (_bankName == "" || _bankName == null);
            do
            {
                Console.Write("Input Bank Address: ");
                _bankAddress = Console.ReadLine();
            } while (_bankAddress == "" || _bankAddress == null);
            Bank bank = new Bank() { BankName = _bankName, BankAddress = _bankAddress};
            await bankRepository.InsertBank(bank);
            return bank.Id;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static async Task BankManagement(Bank bank)
    {
        Customer? customer;
        IEnumerable<Customer> customers;
        IEnumerable<Account> accounts;

        Utils utils = new();

        string[] options2 = new string[] { "0.Exit", "1.Create new customer",
                "2.Create new account", "3.Show List Of Customers",
                "4.Show All Information", "5.List Top 3 Accounts have Largest Balance",
                "6.List All Customer with Increasing Total Balance",
                "7.Show Customer has the most number of transactions (Only on transactions sent)",
                "8.Make a Transaction"};

        bool condition = true;

        Console.WriteLine($"--------------Bank {bank.BankName}--------------");
        do
        {
            switch (utils.ShowOptions(options2))
            {
                case 0:
                    Console.Clear();
                    condition = false;
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("----------------Creating new customer------------------");
                    customer =  await customerRepository.FindCustomerByID(await CreateNewCustomer(bank));
                    if(customer != null)
                    {
                        Console.WriteLine("------Created New Customer Succesfully------");
                    }
                    else
                    {
                        Console.WriteLine("------Created New Customer Fail!!!!------");
                    }
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("----------------List of customers------------------");

                    customers = await customerRepository.GetAllCustomersOfaBank(bank.Id);
                    foreach (var cust in customers)
                    {
                        Console.WriteLine($"Customner ID: {cust.Id}, Customer Name: {cust.CustomerName}, Customer Address: {cust.CustomerAddress}");
                    }

                    Console.Write("Input Customer Id you want to choose: ");
                    int id = -1;
                    if (int.TryParse(Console.ReadLine(), out id))
                    {
                        customer = customers.SingleOrDefault(ct => ct.Id == id);
                        if (customer != null)
                        {
                            Console.Clear();
                            var account = await accountRepository.FindAccountByID(await CreateNewAccount(customer));
                            if (account != null)
                            {
                                Console.WriteLine("Account Added Successfully");
                            }
                            else
                            {
                                Console.WriteLine("Account Added Fail!!!!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Customer found!!!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Customer Id!!!");
                    }

                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("----------------List of customers------------------");
                    customers = await customerRepository.GetAllCustomersOfaBank(bank.Id);
                    foreach (var cust in customers)
                    {
                        Console.WriteLine($"Customner ID: {cust.Id}, Customer Name: {cust.CustomerName}, Customer Address: {cust.CustomerAddress}");
                    }
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("----------------List of customers------------------");
                    customers = await customerRepository.GetAllCustomersOfaBank(bank.Id);
                    foreach (var cust in customers)
                    {
                        Console.WriteLine($"Customner ID: {cust.Id}, Customer Name: {cust.CustomerName}, Customer Address: {cust.CustomerAddress}");
                        Console.WriteLine($"Number Of Account: {cust.Accounts.Count}");
                        foreach(var account in cust.Accounts)
                        {
                            Console.WriteLine($"Account ID: {account.Id}, Balance: {account.Balance}");
                        }
                        Console.WriteLine("-------------------------------------------");
                    }
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("----------------List of Top 3 Accounts------------------");
                    accounts = await accountRepository.GetAllAccountOfaBank(bank.Id);
                    foreach(var account in accounts.OrderByDescending(a => a.Balance).Take(3))
                    {
                        Console.WriteLine($"Account ID: {account.Id}, Balance: {account.Balance}");
                    }
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("----------------List All Customer with Increasing Total Balance------------------");
                    customers = await customerRepository.GetAllCustomersOfaBank(bank.Id);
                    customers.Select(c => new
                    {
                        customer = c,
                        TotalBalance = c.Accounts.Sum(a => a.Balance)
                    })
                    .OrderBy(c => c.TotalBalance)
                    .ToList()
                    .ForEach(item => Console.WriteLine($"Customner ID: {item.customer.Id}, Customer Name: {item.customer.CustomerName}, Customer Address: {item.customer.CustomerAddress}, Total Balance: {item.TotalBalance}"));
                    break;
                case 7:
                    Console.Clear();
                    Console.WriteLine("----------------Customer has the most number of transactions------------------");
                    customers = await customerRepository.GetAllCustomersOfaBank(bank.Id);
                    var customer1 = customers.Select(c => new
                    {
                        customer = c,
                        NumberOfTransaction = c.Accounts
                                .SelectMany(a => a.Transactions)
                                .Count()
                    })
                    .OrderByDescending(c => c.NumberOfTransaction)
                    .FirstOrDefault();
                    if (customer1 != null)
                    {
                        Console.WriteLine($"Customer Id: {customer1.customer.Id}, Customer Name: {customer1.customer.CustomerName}");
                        Console.WriteLine($"Number of Transactions: {customer1.NumberOfTransaction}");
                    }
                    break;
                case 8:
                    Console.Clear();
                    Console.WriteLine("----------------List of Accounts------------------");

                    accounts = await accountRepository.GetAllAccountOfaBank(bank.Id);
                    foreach (var acc in accounts)
                    {
                        Console.WriteLine($"Customer Id: {acc.CustomerId}, Account ID: {acc.Id}, Account Balance: {acc.Balance}");
                    }

                    Console.WriteLine("----------------Making a Transaction------------------");
                    int i1;
                    int i2;

                    Console.Write("Input account id that you want to make a transaction: ");
                    string? id1 = Console.ReadLine();
                    Console.Write("Input account id of receiver: ");
                    string? id2 = Console.ReadLine();

                    if (int.TryParse(id1, out i1) && int.TryParse(id2, out i2))
                    {
                        var account1 = await accountRepository.FindAccountByID(i1);
                        var account2 = await accountRepository.FindAccountByID(i2);
                        if (account1 != null && account2 != null)
                        {
                            decimal _money = new decimal();
                            Console.Write("Input Money(require at least 10): ");
                            if (decimal.TryParse(Console.ReadLine(), out _money))
                            {
                                if (_money >= 10)
                                {
                                    Transaction transaction = new Transaction() {Money = _money,  
                                        TransactionDate = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd")), 
                                        AccountFrom = account1, 
                                        AccountTo = account2};
                                    await transactionRepository.InsertTransaction(transaction);
                                }
                                else
                                {
                                    Console.WriteLine("Money is not enough for a transaction!!!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Input!!!!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No Account found!!!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Account Id!!!");
                    }
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Invalid input, please input again!!!");
                    break;
            }
        } while (condition);
    }

    public static async Task<int> CreateNewCustomer(Bank bank)
    {
        string? _customerName;
        string? _customerAddress;
        try
        {
            do
            {
                Console.Write("Input Customer Name: ");
                _customerName = Console.ReadLine();
            } while (_customerName == "" || _customerName == null);
            do
            {
                Console.Write("Input Customer Address: ");
                _customerAddress = Console.ReadLine();
            } while (_customerAddress == "" || _customerAddress == null);
            Customer customer = new Customer() { CustomerName = _customerName, CustomerAddress = _customerAddress, Bank = bank};
            await customerRepository.InsertCustomer(customer);
            return customer.Id;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public static async Task<int> CreateNewAccount(Customer customer)
    {
        decimal _balance = new decimal();
        try
        {
            Console.Write("Input Balance(require at least 100): ");
            if (decimal.TryParse(Console.ReadLine(), out _balance))
            {
                if(_balance >= 100)
                {
                    Account account = new Account() { Balance = _balance, Customer = customer };
                    await accountRepository.InsertAccount(account);
                    return account.Id;
                }
                else
                {
                    Console.WriteLine("Balance is not enough!!!");
                }
            }
            else
            {
                Console.WriteLine("Invalid Input!!!!");
            }
            return 0;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}