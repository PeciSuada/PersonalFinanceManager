using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;
namespace Api.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _context;

    public TransactionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> GetAll(int? categoryId, DateTime? from, DateTime? to)
    {
        var query = _context.Transactions.Include(t => t.Category).AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }
        if (from.HasValue)
        {
            query = query.Where(t => t.Date >= from.Value.Date);
        }
        if (to.HasValue)
        {
            query = query.Where(t => t.Date < to.Value.Date.AddDays(1));
        }

        return await query.ToListAsync();
    }

    public async Task<Transaction?> GetById(int id)
    {
        return await _context.Transactions.Include(t => t.Category).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Transaction> AddTransaction(Transaction item)
    {
        _context.Transactions.Add(item); 
        
        await _context.SaveChangesAsync();
        
        return item;
    }

    public async Task<bool> ChangeTransaction(int id, Transaction item)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return false;
        }

        transaction.Amount = item.Amount;
        transaction.Description = item.Description;
        transaction.Date = item.Date;
        transaction.Type = item.Type;
        transaction.CategoryId = item.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTransaction(int id)
    {
        var transaction = await _context.Transactions.FindAsync(id);
        if (transaction == null)
        {
            return false;
        }

        _context.Transactions.Remove(transaction);
       
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Summary> GetSummary(DateTime? from, DateTime? to)
    {
        var query = _context.Transactions.AsQueryable();

        if (from.HasValue)
        {
            query = query.Where(t => t.Date >= from.Value.Date);
        }
        if (to.HasValue)
        {
            query = query.Where(t => t.Date < to.Value.Date.AddDays(1));
        }

        var income = await query.Where(t => t.Type == TransactionType.Income).SumAsync(t => t.Amount);
        
        var expense = await query.Where(t => t.Type == TransactionType.Expense).SumAsync(t => t.Amount);

        return new Summary{TotalIncome = income,TotalExpense = expense,Balance = income - expense};
    }
}