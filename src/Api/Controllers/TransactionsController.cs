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

static Category foodCategory=new Category {
Id=1,
Name="Groceries"
};

static Category electronicsCategory=new Category {
Id=2,
Name="Electronics"
};

static Category billsCategory= new Category{
Id=3,
Name="Bills"
};  

static List<Transaction> Transactions = [

new Transaction {
Id = 101,
Amount = 45.50m,
Description = "Weekly grocery shopping at Supermarket",
Date = DateTime.Now,
Type = TransactionType.Expense,
CategoryId = foodCategory.Id,
Category = foodCategory 
},

new Transaction {
Id=102,
Amount=1000.5m,
Description="New Tv",
Date= DateTime.Now,
Type= TransactionType.Expense,
CategoryId=electronicsCategory.Id,
Category=electronicsCategory
},

new Transaction{
Id=103,
Amount=121.5m,
Description="Monthly bills",
Date= DateTime.Now,
Type= TransactionType.Expense,
CategoryId=billsCategory.Id,
Category=billsCategory
},
new Transaction{
    Id=104,
    Amount=1500m,
    Description="Salary",
    Date=DateTime.Now,
    Type=TransactionType.Income,
    CategoryId=foodCategory.Id,
    Category=foodCategory
}

];

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

int maxid=0;
foreach(var i in Transactions){

if(i.Id > maxid){
maxid=i.Id;
}
}
maxid +=1;
item.Id=maxid;


Transactions.Add(item);
return CreatedAtAction(nameof(GetTransactionById), new{id= item.Id}, item);
 }  


 [HttpPut("{id}")]
 public IActionResult  ChangeTransaction (int id, Transaction item){
 
 foreach(var i in Transactions){
if(id == i.Id){
    i.Amount=item.Amount;
    i.Description=item.Description;
    i.Date=item.Date;
    i.Type=item.Type;
    i.CategoryId=item.CategoryId;

    return NoContent();
}

 
 }

return NotFound();
 }



 [HttpDelete("{id}")]
 public IActionResult DeleteTransaction (int id){

foreach(var i in Transactions){
if(i.Id== id){
Transactions.Remove(i);
return NoContent();
}
}
return NotFound();
 }


[HttpGet("summary")]

public ActionResult<Summary> GetSummary(DateTime? from, DateTime? to){

List<Transaction> Result=Transactions;

if(from.HasValue){

Result= Result.Where(item=> item.Date.Date>=from).ToList();  
}

if(to.HasValue){
Result= Result.Where(item=> item.Date.Date<=to).ToList();
}


decimal income = Result.Where(item=>item.Type==TransactionType.Income).Sum(item=>item.Amount);
decimal expense = Result.Where(item=>item.Type==TransactionType.Expense).Sum(item=>item.Amount);
decimal balance = income-expense;
return Ok(new Summary {TotalIncome= income, TotalExpense= expense, Balance= balance});

}










}

