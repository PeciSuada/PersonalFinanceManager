using Api.Models;
using Api.Services;
namespace Api.Services;

public class CategoryService : ICategoryService{
    

static List<Category> Categories=[

new Category{
Id=1,
Name="Groceries"

},

new Category{
Id=2,
Name="Electronics"

},

new Category{
Id=3,
Name="Bills"
},

new Category{
Id=4,
Name="Earnings"
}

];

public List<Category> GetCategories(){
return Categories;

}


public Category?  GetCategoryById(int id){
 
        foreach(var item in Categories){
            if(item.Id==id){
            return item;
            }
        }

return null;
  }




  public Category AddCategory(Category item){

int maxid=0;

foreach(var i in Categories){
if(i.Id> maxid){
maxid=i.Id;
}
}
maxid+=1;
item.Id=maxid;

Categories.Add(item);
return item;
}


public bool ChangeCategory(int id, Category item){


 foreach(var i in Categories){
if(id == i.Id){
    i.Name=item.Name;
    return true;
}

}
return false;
}

 public bool DeleteCategory (int id){

foreach(var i in Categories){
if(i.Id== id){
Categories.Remove(i);
return true;
}
}
return false;
 }

}