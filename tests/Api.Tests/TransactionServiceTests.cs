using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Identity.Client;

public class TransactionServiceTests
{
    private AppDbContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

   [Fact]
public async Task AddTransaction_SavesTransactionAndAssignsId()
{
    using var context = CreateContext();
    var categoryService = new CategoryService(context);
    var service = new TransactionService(context);

    var category = new Category { Name = "Food" };
    var added = await categoryService.AddCategory(category);

    var transaction = new Transaction
    {
        Amount = 20.00m,
        Description = "Coffee",
        Date = DateTime.Now,
        Type = TransactionType.Expense,
        CategoryId = added.Id
    };

    var result = await service.AddTransaction(transaction);

    Assert.True(result.Id > 0);
    Assert.Equal("Coffee", result.Description);
}

    [Fact]
    public async Task GetById_ReturnsTransactionWithCategory_WhenTransactionExists()
    {
        using var context = CreateContext();
    var categoryService = new CategoryService(context);
    var service = new TransactionService(context);

    var category = new Category { Name = "Food" };
    var added = await categoryService.AddCategory(category);

    var transaction = new Transaction
    {
        Amount = 20.00m,
        Description = "Coffee",
        Date = DateTime.Now,
        Type = TransactionType.Expense,
        CategoryId = added.Id
    };

    var addedT = await service.AddTransaction(transaction);

    var result= await service.GetById(addedT.Id);

    Assert.NotNull(result);
    Assert.Equal("Coffee", result.Description);
     Assert.NotNull(result.Category);
Assert.Equal("Food", result.Category.Name);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenTransactionDoesNotExist()
    {
         using var context = CreateContext();
    var categoryService = new CategoryService(context);
    var service = new TransactionService(context);

    var result= await service.GetById(999);
       Assert.Null(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOnlyTransactionsOfCategory_WhenCategoryIdIsGiven()
    {

        
        using var context = CreateContext();
    var categoryService = new CategoryService(context);
    var service = new TransactionService(context);

    var category1 = new Category { Name = "Drinks" };
    var addedC1 = await categoryService.AddCategory(category1);

    
    var category2 = new Category { Name = "Food" };
    var addedC2 = await categoryService.AddCategory(category2);


    var transaction1 = new Transaction
    {
        Amount = 20.00m,
        Description = "Coffee",
        Date = DateTime.Now,
        Type = TransactionType.Expense,
        CategoryId = addedC1.Id
    };

        var addedT1 = await service.AddTransaction(transaction1);

    var transaction2 = new Transaction
    {
        Amount = 16.00m,
        Description = "Chocolate",
        Date = DateTime.Now,
        Type = TransactionType.Expense,
        CategoryId = addedC2.Id
    };

        var addedT2 = await service.AddTransaction(transaction2);


    var result= await service.GetAll(addedC1.Id,null,null);

    Assert.Single(result);
    Assert.Equal(addedC1.Id, result[0].CategoryId);
    }

[Fact]
public async Task GetAll_ReturnsOnlyTransactionsInRange_WhenFromAndToAreGiven()
{
    using var context = CreateContext();
    var categoryService = new CategoryService(context);
    var service = new TransactionService(context);

    var category = new Category { Name = "Food" };
    var addedC = await categoryService.AddCategory(category);

    var before = new Transaction
    {
        Amount = 10m,
        Description = "Before",
        Date = new DateTime(2026, 10, 1, 10, 0, 0),
        Type = TransactionType.Expense,
        CategoryId = addedC.Id
    };
    var middle = new Transaction
    {
        Amount = 20m,
        Description = "Middle",
        Date = new DateTime(2026, 10, 5, 12, 0, 0),
        Type = TransactionType.Expense,
        CategoryId = addedC.Id
    };
    var lastDay = new Transaction
    {
        Amount = 30m,
        Description = "LastDay",
        Date = new DateTime(2026, 10, 10, 15, 0, 0),
        Type = TransactionType.Expense,
        CategoryId = addedC.Id
    };
    var after = new Transaction
    {
        Amount = 40m,
        Description = "After",
        Date = new DateTime(2026, 10, 11, 9, 0, 0),
        Type = TransactionType.Expense,
        CategoryId = addedC.Id
    };

    await service.AddTransaction(before);
    await service.AddTransaction(middle);
    await service.AddTransaction(lastDay);
    await service.AddTransaction(after);

    var result = await service.GetAll(null, new DateTime(2026, 10, 3), new DateTime(2026, 10, 10));

    Assert.Equal(2, result.Count);
    Assert.DoesNotContain(result, t => t.Description == "Before");
    Assert.DoesNotContain(result, t => t.Description == "After");
    Assert.Contains(result, t => t.Description == "LastDay");
}

[Fact]
public async Task ChangeTransaction_ReturnsTrue_WhenTransactionExists()
{
    using var context = CreateContext();
    var categoryService = new CategoryService(context);
    var service = new TransactionService(context);

    var category = new Category { Name = "Food" };
    var addedC = await categoryService.AddCategory(category);

    var transaction = new Transaction
    {
        Amount = 20m,
        Description = "Coffee",
        Date = new DateTime(2026, 10, 1),
        Type = TransactionType.Expense,
        CategoryId = addedC.Id
    };
    var addedT = await service.AddTransaction(transaction);

    var updated = new Transaction
    {
        Amount = 50m,
        Description = "Tea",
        Date = new DateTime(2026, 10, 2),
        Type = TransactionType.Income,
        CategoryId = addedC.Id
    };

    var result = await service.ChangeTransaction(addedT.Id, updated);

    Assert.True(result);

    context.ChangeTracker.Clear();
    var check = await service.GetById(addedT.Id);
    Assert.NotNull(check);
    Assert.Equal(50m, check.Amount);
    Assert.Equal("Tea", check.Description);
    Assert.Equal(TransactionType.Income, check.Type);
}

[Fact]
public async Task ChangeTransaction_ReturnsFalse_WhenTransactionDoesNotExist()
{
    using var context = CreateContext();
    var service = new TransactionService(context);

    var updated = new Transaction
    {
        Amount = 50m,
        Description = "Tea",
        Date = new DateTime(2026, 10, 2),
        Type = TransactionType.Income,
        CategoryId = 1
    };

    var result = await service.ChangeTransaction(999, updated);

    Assert.False(result);
}

[Fact]
public async Task DeleteTransaction_ReturnsTrue_WhenTransactionExists()
{
    using var context = CreateContext();
    var categoryService = new CategoryService(context);
    var service = new TransactionService(context);

    var category = new Category { Name = "Food" };
    var addedC = await categoryService.AddCategory(category);

    var transaction = new Transaction
    {
        Amount = 20m,
        Description = "Coffee",
        Date = new DateTime(2026, 10, 1),
        Type = TransactionType.Expense,
        CategoryId = addedC.Id
    };
    var addedT = await service.AddTransaction(transaction);

    var result = await service.DeleteTransaction(addedT.Id);

    Assert.True(result);

    var check = await service.GetById(addedT.Id);
    Assert.Null(check);
}

[Fact]
public async Task DeleteTransaction_ReturnsFalse_WhenTransactionDoesNotExist()
{
    using var context = CreateContext();
    var service = new TransactionService(context);

    var result = await service.DeleteTransaction(999);

    Assert.False(result);
}

[Fact]
public async Task GetSummary_CalculatesIncomeExpenseAndBalance()
{
    using var context = CreateContext();
    var categoryService = new CategoryService(context);
    var service = new TransactionService(context);

    var category = new Category { Name = "Food" };
    var addedC = await categoryService.AddCategory(category);

    var income = new Transaction
    {
        Amount = 1200m,
        Description = "Salary",
        Date = new DateTime(2026, 10, 3),
        Type = TransactionType.Income,
        CategoryId = addedC.Id
    };
    var expense1 = new Transaction
    {
        Amount = 20m,
        Description = "Coffee",
        Date = new DateTime(2026, 10, 3),
        Type = TransactionType.Expense,
        CategoryId = addedC.Id
    };
    var expense2 = new Transaction
    {
        Amount = 80.50m,
        Description = "Headphones",
        Date = new DateTime(2026, 10, 4),
        Type = TransactionType.Expense,
        CategoryId = addedC.Id
    };

    await service.AddTransaction(income);
    await service.AddTransaction(expense1);
    await service.AddTransaction(expense2);

    var result = await service.GetSummary(null, null);

    Assert.Equal(1200m, result.TotalIncome);
    Assert.Equal(100.50m, result.TotalExpense);
    Assert.Equal(1099.50m, result.Balance);
}

[Fact]
public async Task GetSummary_ReturnsZeros_WhenThereAreNoTransactions()
{
    using var context = CreateContext();
    var service = new TransactionService(context);

    var result = await service.GetSummary(null, null);

    Assert.Equal(0m, result.TotalIncome);
    Assert.Equal(0m, result.TotalExpense);
    Assert.Equal(0m, result.Balance);
}






}