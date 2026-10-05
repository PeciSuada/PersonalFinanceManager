using Api.DTOs;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;
    private readonly ICategoryService _categoryService;

    public TransactionsController(ITransactionService service, ICategoryService categoryService)
    {
        _service = service;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Transaction>>> GetTransactions(int? categoryId, DateTime? from, DateTime? to)
    {
        return Ok(await _service.GetAll(categoryId, from, to));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransactionById(int id)
    {
        var transaction = await _service.GetById(id);
        if (transaction == null)
        {
            return NotFound();
        }
        return Ok(transaction);
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> AddTransaction(CreateTransactionDto item)
    {
        if (await _categoryService.GetCategoryById(item.CategoryId) == null)
        {
            return BadRequest("Category does not exist.");
        }

        var newTransaction = new Transaction
        {
            Amount = item.Amount,
            Description = item.Description,
            Date = item.Date,
            Type = item.Type,
            CategoryId = item.CategoryId
        };

        var transaction = await _service.AddTransaction(newTransaction);
        return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ChangeTransaction(int id, CreateTransactionDto item)
    {
        if (await _categoryService.GetCategoryById(item.CategoryId) == null)
        {
            return BadRequest("Category does not exist.");
        }

        var newTransaction = new Transaction
        {
            Amount = item.Amount,
            Description = item.Description,
            Date = item.Date,
            Type = item.Type,
            CategoryId = item.CategoryId
        };

        var found = await _service.ChangeTransaction(id, newTransaction);
        if (found)
        {
            return NoContent();
        }
        return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var found = await _service.DeleteTransaction(id);
        if (found)
        {
            return NoContent();
        }
        return NotFound();
    }

    [HttpGet("summary")]
    public async Task<ActionResult<Summary>> GetSummary(DateTime? from, DateTime? to)
    {
        var summary = await _service.GetSummary(from, to);
        return Ok(summary);
    }
}