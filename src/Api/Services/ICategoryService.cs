using Api.Models;
using Api.Services;
namespace Api.Services;


public interface ICategoryService{

Task <List<Category>> GetCategories();

 public Task <Category?> GetCategoryById(int id);

  public Task <Category> AddCategory(Category item);

  public Task <bool> ChangeCategory(int id, Category item);

   public Task <bool> DeleteCategory (int id);

}