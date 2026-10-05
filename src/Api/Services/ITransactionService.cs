using Api.Models;
namespace Api.Services;

public interface ITransactionService
{
    Task<List<Transaction>> GetAll(int? categoryId, DateTime? from, DateTime? to);


   public  Task<Transaction?> GetById(int id);


     public Task<Transaction> AddTransaction(Transaction item);

    public  Task<bool> ChangeTransaction(int id, Transaction item);


    public  Task<bool> DeleteTransaction(int id);


    public  Task<Summary> GetSummary(DateTime? from, DateTime? to);
}