using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

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
 public ActionResult<Transaction> AddTransaction(Transaction item){
var transaction=_service.AddTransaction(item);

return CreatedAtAction(nameof(GetTransactionById), new{id= transaction.Id}, transaction);
 }  


 [HttpPut("{id}")]
 public IActionResult  ChangeTransaction (int id, Transaction item){


var found=_service.ChangeTransaction(id, item);

        if (found)
        {
            
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

