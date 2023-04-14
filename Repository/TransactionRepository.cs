using BankManagement.Entities;
using BankManagement.DataAccess;

namespace BankManagement.Repository;

public class TransactionRepository : ITransactionRepository
{
    public async Task InsertTransaction(Transaction transaction) => await TransactionDAO.Instance.AddNewTransaction(transaction);
}
