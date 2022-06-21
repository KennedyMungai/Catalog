using Microsoft.AspNetCore.Mvc;

using Catalog.Repositories;
using Catalog.Entities;
using Catalog.Dtos;
using Catalog.Extensions;

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
    public IEnumerable<ItemDto> GetItems()
    {
        var items = repository.GetItems().Select(item => item.AsDto());
        return items;
    }

    [HttpGet("{id:int}")]
    public ActionResult<ItemDto> GetItem(Guid id)
    {
        var item = repository.GetItem(id);

        if (item is null)
        {
            return NotFound();
        }

        return item.AsDto();
    }
}