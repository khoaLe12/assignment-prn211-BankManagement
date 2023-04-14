using BankManagement.Entities;

namespace BankManagement.Repository;

public interface ITransactionRepository
{
    Task InsertTransaction(Transaction transaction);
}
