using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController] 
[Route("api/[controller]")]

public class CategoriesController : ControllerBase{

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
}

];

[HttpGet]
public ActionResult<List<Category>> GetCategories(){
return Ok(Categories);

}

[HttpGet("{id}")]
public ActionResult<Category> GetCategoryById(int id){
foreach(var item in Categories){
if(item.Id == id){
    return Ok(item);
}

}
return NotFound();
}

[HttpPost]
public ActionResult<Category> AddCategory(Category item){

int maxid=0;

foreach(var i in Categories){
if(i.Id> maxid){
maxid=i.Id;
}
}
maxid+=1;
item.Id=maxid;

Categories.Add(item);
return CreatedAtAction(nameof(GetCategoryById), new{id= item.Id}, item);
}

[HttpPut("{id}")]

public IActionResult ChangeCategory(int id, Category item){


 foreach(var i in Categories){
if(id == i.Id){
    i.Name=item.Name;
    return NoContent();
}

}
return NotFound();
}

[HttpDelete("{id}")]
 public IActionResult DeleteCategory (int id){

foreach(var i in Categories){
if(i.Id== id){
Categories.Remove(i);
return NoContent();
}
}
return NotFound();
 }
}