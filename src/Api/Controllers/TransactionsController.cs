using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Api.DTOs;

namespace Api.Controllers;

[ApiController] 
[Route("api/[controller]")]

public class TransactionsController : ControllerBase{

    private readonly ITransactionService _service;

    public TransactionsController(ITransactionService option){
    _service= option;
    }

[HttpGet]
public ActionResult<List<Transaction>> GetTransactions(int? categoryId, DateTime? from, DateTime? to){


return Ok(_service.GetAll(categoryId, from, to));
}

[HttpGet("{id}")]
public ActionResult<Transaction> GetTransactionById(int id){

var transaction=_service.GetById(id);

if(transaction== null){
return NotFound();   
        }
return Ok(transaction);
}


[HttpPost]
 public ActionResult<Transaction> AddTransaction(CreateTransactionDto item){

var newTransaction = new Transaction {
    Amount = item.Amount,
    Description = item.Description,
    Date = item.Date,
    Type = item.Type,
    CategoryId = item.CategoryId
};
var transaction = _service.AddTransaction(newTransaction);
return CreatedAtAction(nameof(GetTransactionById), new{id= newTransaction.Id}, newTransaction);
 }  


 [HttpPut("{id}")]
 public IActionResult  ChangeTransaction (int id, CreateTransactionDto item){


var newTransaction = new Transaction {
    Amount = item.Amount,
    Description = item.Description,
    Date = item.Date,
    Type = item.Type,
    CategoryId = item.CategoryId
};

var found=_service.ChangeTransaction(id, newTransaction);

        if (found){
            
    return NoContent();
        }
        return NotFound();

 }



 [HttpDelete("{id}")]
 public IActionResult DeleteTransaction (int id){

 var found= _service.DeleteTransaction(id);

if(found){

return NoContent();
}

return NotFound();
 }


[HttpGet("summary")]

public ActionResult<Summary> GetSummary(DateTime? from, DateTime? to){

var summary=_service.GetSummary(from,to);

return Ok(summary);

}










}

