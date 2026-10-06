using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class CategoryServiceTests
{
    private AppDbContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task AddCategory_WereIdIsBiggerThan_0()
    {
        using var context = CreateContext();
        var service = new CategoryService(context);

        var category = new Category { Name = "Groceries" };
        var result = await service.AddCategory(category);

        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task GetCategories_ReturnEmptyList()
    {
        using var context = CreateContext();
        var service = new CategoryService(context);

        var result = await service.GetCategories();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsNull_WhenCategoryDoesNotExist()
    {
        using var context = CreateContext();
        var service = new CategoryService(context);

        var result = await service.GetCategoryById(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsCategory_WhenCategoryExists()
    {
        using var context = CreateContext();
        var service = new CategoryService(context);

        var newCategory = new Category { Name = "Books" };
        var added = await service.AddCategory(newCategory);

        var result = await service.GetCategoryById(added.Id);

        Assert.NotNull(result);
        Assert.Equal("Books", result.Name);
    }

    [Fact]
    public async Task ChangeCategory_ReturnsTrue_WhenCategoryExists()
    {
        using var context = CreateContext();
        var service = new CategoryService(context);

        var original = new Category { Name = "Drinks" };
        var added = await service.AddCategory(original);
        var updated = new Category { Name = "Books" };

        var result = await service.ChangeCategory(added.Id, updated);

        Assert.True(result);

        var check = await service.GetCategoryById(added.Id);
        Assert.NotNull(check);
        Assert.Equal("Books", check.Name);
    }

    [Fact]
    public async Task ChangeCategory_InvalidIdReturnsFalse()
    {
        using var context = CreateContext();
        var service = new CategoryService(context);

        var category = new Category { Name = "Medication" };
        var add = await service.AddCategory(category);

        var result = await service.ChangeCategory(999, category);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteCategory_DeleteCategoryWithoutTransactions()
    {
        using var context = CreateContext();
        var service = new CategoryService(context);

        var category = new Category { Name = "Food" };
        var added = await service.AddCategory(category);

        var deleted = await service.DeleteCategory(added.Id);

        Assert.True(deleted);

        var check = await service.GetCategoryById(added.Id);
        Assert.Null(check);
    }

    [Fact]
    public async Task DeleteCategory_DeleteCategoryWithInvalidId()
    {
        using var context = CreateContext();
        var service = new CategoryService(context);

        var deleted = await service.DeleteCategory(999);

        Assert.False(deleted);
    }

    [Fact]
    public async Task DeleteCategory_ThrowsInvalidOperationException_WhenCategoryHasTransactions()
    {
        using var context = CreateContext();
        var service = new CategoryService(context);

        var category = new Category { Name = "Food" };
        var added = await service.AddCategory(category);

        var transaction = new Transaction
        {
            Amount = 20.00m,
            Description = "Coffee",
            Date = DateTime.Now,
            Type = TransactionType.Expense,
            CategoryId = added.Id
        };
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.DeleteCategory(added.Id));

        var check = await service.GetCategoryById(added.Id);
        Assert.NotNull(check);
    }
}