using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Api.DTOs;

namespace Api.Controllers;

[ApiController] 
[Route("api/[controller]")]

public class CategoriesController : ControllerBase{

private readonly  ICategoryService _service;
public  CategoriesController(ICategoryService option)
    {
_service = option;

    }




[HttpGet]
public async Task<ActionResult<List<Category>>> GetCategories(){
    var category= await _service.GetCategories();
   return Ok(category);


}

[HttpGet("{id}")]
public async Task <ActionResult<Category>> GetCategoryById(int id){

var category= await _service.GetCategoryById(id);
if(category==null){
   return NotFound();
}
 return Ok(category);

}

[HttpPost]
public async Task<ActionResult<Category>> AddCategory(CreateCategoryDto item){
  
    var NewCategory = new Category
    {
        Name=item.Name
    };

var category=await _service.AddCategory(NewCategory);

return CreatedAtAction(nameof(GetCategoryById), new{id= NewCategory.Id}, NewCategory);
}



[HttpPut("{id}")]

public async Task <IActionResult> ChangeCategory(int id, CreateCategoryDto item){

 var NewCategory = new Category
    {
        Name=item.Name
    };
var found=await _service.ChangeCategory(id,NewCategory);

if(found){

    return NoContent();
}
return NotFound();
}

[HttpDelete("{id}")]
public async Task<IActionResult> DeleteCategory(int id)
{
    try
    {
        var found = await _service.DeleteCategory(id);

        if (found)
        {
            return NoContent();
        }
        return NotFound();
    }
    catch (InvalidOperationException ex)
    {
        return Conflict(ex.Message);
    }
}
 
}