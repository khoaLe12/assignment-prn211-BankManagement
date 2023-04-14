using BankManagement.Entities;
using BankManagement.DataAccess;

namespace BankManagement.Repository;

public class CustomerRepository : ICustomerRepository
{
    public async Task<Customer?> FindCustomerByID(int cusId) => await CustomerDAO.Instance.GetCustomerByID(cusId);

    public async Task<IEnumerable<Customer>> GetAllCustomers() => await CustomerDAO.Instance.GetAllCustomers();

    public async Task<IEnumerable<Customer>> GetAllCustomersOfaBank(int bankId)
    {
        var customers = await CustomerDAO.Instance.GetAllCustomers();
        return customers.Where(ct => ct.Bank.Id==bankId);
    }

    public async Task InsertCustomer(Customer customer) => await CustomerDAO.Instance.AddNewCustomer(customer);
}
