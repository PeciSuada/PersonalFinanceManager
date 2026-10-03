using Api.Models;
namespace Api.Services;

public class TransactionService : ITransactionService{

 
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


public Transaction? GetById(int id)
    {
        foreach(var item in Transactions)
        {
            if(item.Id==id){
            return item;
            }
        }

return null;
    }






public List<Transaction> GetAll(int? categoryId, DateTime? from, DateTime? to){

 List<Transaction> Result=Transactions;

if(categoryId.HasValue){

Result=Result.Where(item=>item.CategoryId==categoryId).ToList();
}

if(from.HasValue){
Result=Result.Where(item=>item.Date.Date>=from).ToList();
}


if(to.HasValue){
Result=Result.Where(item=>item.Date.Date<=to).ToList();

}

return Result;
}
   
}
