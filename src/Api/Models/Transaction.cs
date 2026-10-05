using System.ComponentModel.DataAnnotations.Schema;
namespace Api.Models;


public class Transaction{

public int Id{
    get;
    set;
}

[Column(TypeName = "decimal(18,2)")]
public decimal Amount{
    get;
    set;
}

public string Description{
    get;
    set;
}=string.Empty;

public DateTime Date{
    get;
    set;
}

public TransactionType Type{
    get;
    set;
}


public int CategoryId{
    get;
    set;
}

public Category? Category{
    get;
    set;
}

}



