using Api.Models;
using Api.Services;
namespace Api.Services;
using Api.Data;
using Microsoft.EntityFrameworkCore;
public class CategoryService : ICategoryService{
    

    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context=context;
    }


public async Task <List<Category>> GetCategories(){
return await _context.Categories.ToListAsync();

}


public async Task <Category?>  GetCategoryById(int id){
 
return await _context.Categories.FindAsync(id);  }




  public async Task <Category> AddCategory(Category item){

_context.Categories.Add(item);
await _context.SaveChangesAsync();
return item;
}


public async Task<bool> ChangeCategory(int id, Category item){

var category= await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return false;
        }
category.Name=item.Name;

await _context.SaveChangesAsync();
return true;

}

 public async Task<bool> DeleteCategory (int id){
var category= await _context.Categories.FindAsync(id);
 if (category == null)
        {
            return false;
        }


        if (await _context.Transactions.AnyAsync(t => t.CategoryId == id))
{
    throw new InvalidOperationException("Category has transactions.");
}


        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
}

    public async Task AddTransaction(Transaction transaction)
    {
        throw new NotImplementedException();
    }
}