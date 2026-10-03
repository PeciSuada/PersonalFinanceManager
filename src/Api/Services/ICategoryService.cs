using Api.Models;
using Api.Services;
namespace Api.Services;


public interface ICategoryService{

 List<Category>GetCategories();

 public Category? GetCategoryById(int id);

  public Category AddCategory(Category item);

  public bool ChangeCategory(int id, Category item);

   public bool DeleteCategory (int id);

}