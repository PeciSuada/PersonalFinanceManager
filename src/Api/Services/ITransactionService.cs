using Api.Models;
using Api.Services;
namespace Api.Services;


public interface ITransactionService{

List <Transaction> GetAll(int? categoryId, DateTime? from, DateTime? to);

Transaction? GetById(int id);

}