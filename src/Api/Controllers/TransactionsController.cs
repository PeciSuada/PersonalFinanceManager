using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController] 
[Route("api/[controller]")]

public class TransactionsController : ControllerBase{

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
}

];

[HttpGet]
public ActionResult<List<Transaction>> GetTransactions(int? categoryId){

if(categoryId.HasValue){

var Filtered =Transactions.Where(transaction=>transaction.CategoryId== categoryId);

List<Transaction>Result=Filtered.ToList();

return Ok(Result);
}


return Ok(Transactions);
}

[HttpGet("{id}")]
public ActionResult<Transaction> GetTransactionById(int id){

foreach(var item in Transactions){
    if( item.Id== id )
    return Ok(item);


}
return NotFound();
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
}

