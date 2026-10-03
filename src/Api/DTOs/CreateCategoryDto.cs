
using System.ComponentModel.DataAnnotations;
using Api.Models;
namespace Api.DTOs;

public class CreateCategoryDto
{

[Required(ErrorMessage ="Name required")]
  public string Name{
        
        get;
        set;

    }=string.Empty;


}