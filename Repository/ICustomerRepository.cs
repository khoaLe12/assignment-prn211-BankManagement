using BankManagement.Entities;

namespace BankManagement.Repository;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAllCustomers();
    Task<IEnumerable<Customer>> GetAllCustomersOfaBank(int bankId);
    Task InsertCustomer(Customer customer);
    Task<Customer?> FindCustomerByID(int cusId);
}
