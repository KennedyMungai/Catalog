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
    private readonly IItemsRepository? repository;

    public ItemsController(IItemsRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public IEnumerable<ItemDto> GetItems()
    {
        var items = repository.GetItemsAsync().Select(item => item.AsDto());
        return items;
    }

    [HttpGet("{id:int}")]
    public ActionResult<ItemDto> GetItem(Guid id)
    {
        var item = repository.GetItemAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        return item.AsDto();
    }

    [HttpPost]
    public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        repository.CreateItemAsync(item);

        return CreatedAtAction(nameof(GetItem), new {id = item.Id}, item.AsDto());
    }

    //PUT /items/{id}
    [HttpPut("{id}")]
    public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
    {
        var existingItem  = repository.GetItemAsync(id);

        if(existingItem is null)
        {
            return NotFound();
        }

        Item updatedItem = existingItem with {
            Name = itemDto.Name, 
            Price = itemDto.Price
        };

        repository.UpdateItemAsync(updatedItem);

        return NoContent();
    }

    //DELETE /Items/{id}
    [HttpDelete("{id}")]
    public ActionResult DeleteItem(Guid id)
    {
        var existingItem  = repository.GetItemAsync(id);

        if(existingItem is null)
        {
            return NotFound();
        }

        repository.DeleteItemAsync(id);

        return NoContent();
    }
}