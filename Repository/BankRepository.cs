using BankManagement.Entities;
using BankManagement.DataAccess;

namespace BankManagement.Repository;


public class BankRepository : IBankRepository
{
    public async Task GetAllBank()
    {
        var banks = await BankDAO.Instance.GetBankList();
        foreach (var bank in banks)
        {
            Console.WriteLine($"Bank ID: {bank.Id}, Bank Name: {bank.BankName}, Bank Address: {bank.BankAddress}");
        }
    }

    public async Task InsertBank(Bank bank) => await BankDAO.Instance.AddNewBank(bank);

    public async Task<Bank?> FindBankByID(int bankId) => await BankDAO.Instance.GetBankByID(bankId);
}
