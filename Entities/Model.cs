using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagement.Entities;

public class Bank
{
    public int Id { get; set; }

    [MaxLength(20)]
    public string BankName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string BankAddress { get; set; } = string.Empty;

    public List<Customer> Customers { get; set; } = new();
}

public class Customer
{
    public int Id { get; set; }

    [MaxLength(20)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string CustomerAddress { get; set; } = string.Empty;

    public Bank Bank { get; set; } = new();

    [Required]
    public int BankId { get; set; }

    public List<Account> Accounts { get; set; } = new();
}

public class Account
{
    public int Id { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Balance { get; set; }

    public Customer Customer { get; set; } = new();

    [Required]
    public int CustomerId { get; set; }

    public List<Transaction> Transactions { get; set; } = new();

    public List<Transaction> TransactionsReceived { get; set; } = new();
}

public class Transaction
{
    public int Id { get; set; }

    [Column(TypeName = "decimal(8, 2)")]
    public decimal Money { get; set; }

    [Column(TypeName = "date")]
    public DateTime TransactionDate { get; set; }

    public Account AccountFrom { get; set; } = new();

    public int AccountFromID { get; set; }

    public Account AccountTo { get; set; } = new();

    public int AccountToId { get; set; }
}
