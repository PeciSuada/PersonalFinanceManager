using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Api.DTOs;

namespace Api.Controllers;

[ApiController] 
[Route("api/[controller]")]

public class CategoriesController : ControllerBase{

private readonly  ICategoryService _service;
public CategoriesController(ICategoryService option)
    {
        _service=option;
    }




[HttpGet]
public ActionResult<List<Category>> GetCategories(){
    var category=_service.GetCategories();
   return Ok(category);


}

[HttpGet("{id}")]
public ActionResult<Category> GetCategoryById(int id){

var category=_service.GetCategoryById(id);
if(category==null){
   return NotFound();
}
 return Ok(category);

}

[HttpPost]
public ActionResult<Category> AddCategory(CreateCategoryDto item){
  
    var NewCategory = new Category
    {
        Name=item.Name
    };

var category=_service.AddCategory(NewCategory);

return CreatedAtAction(nameof(GetCategoryById), new{id= NewCategory.Id}, NewCategory);
}



[HttpPut("{id}")]

public IActionResult ChangeCategory(int id, CreateCategoryDto item){

 var NewCategory = new Category
    {
        Name=item.Name
    };
var found=_service.ChangeCategory(id,NewCategory);

if(found){

    return NoContent();
}
return NotFound();
}

[HttpDelete("{id}")]
 public IActionResult DeleteCategory (int id){


var found=_service.DeleteCategory(id);

if(found){
return NoContent();
}
return NotFound();
}

 
}