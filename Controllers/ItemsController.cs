using Microsoft.AspNetCore.Mvc;

using Catalog.Repositories;
using Catalog.Entities;

namespace Catalog.Controllers;

//GET /Items
[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IInMemItemsRepository? repository;

    public ItemsController(IInMemItemsRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public IEnumerable<Item> GetItems()
    {
        var items = repository.GetItems();
        return items;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Item> GetItem(Guid id)
    {
        var item = repository.GetItem(id);

        if (item is null)
        {
            return NotFound();
        }

        return item;
    }
}