using Microsoft.AspNetCore.Mvc;

using Catalog.Repositories;
using Catalog.Entities;

namespace Catalog.Controllers;

//GET /Items
[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly InMemItemsRepository? repository;

    public ItemsController()
    {
        repository = new InMemItemsRepository();
    }

    [HttpGet]
    public IEnumerable<Item> GetItems()
    {
        var items = repository.GetItems();
        return items;
    }
}