using Api.Models;
using Api.Services;
namespace Api.Services;


public interface ITransactionService{

List <Transaction> GetAll(int? categoryId, DateTime? from, DateTime? to);

Transaction? GetById(int id);

Transaction AddTransaction(Transaction item);


public bool ChangeTransaction (int id, Transaction item);

 public bool DeleteTransaction (int id);

public Summary GetSummary(DateTime? from, DateTime? to);
    }