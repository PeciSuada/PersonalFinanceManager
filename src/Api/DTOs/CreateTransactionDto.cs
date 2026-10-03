

using System.ComponentModel.DataAnnotations;
using Api.Models;
namespace Api.DTOs;

public class CreateTransactionDto
{

    [Range(0.05, 1000000.0, ErrorMessage = "Amount range not valid")]
        public decimal Amount
    {
        get;
        set;

    }

    [Required(ErrorMessage = "Description required")]
    public string Description
    {
        get;
        set;
       
    }=string.Empty;

    public DateTime Date
    {
        get;
        set;
    }

    public TransactionType Type{
        get;
        set;
    }

    [Range(1, 100, ErrorMessage = "CategoryId required")]
    public int CategoryId
    {
        
        get;
        set;
    }

}