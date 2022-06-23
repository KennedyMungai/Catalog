using Microsoft.AspNetCore.Mvc;

using Catalog.Api.Repositories;
using Catalog.Api.Entities;
using Catalog.Api.Dtos;
using Catalog.Api.Extensions;

namespace Catalog.Api.Controllers;

//GET /Items
[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemsRepository? repository;
    private readonly ILogger<ItemsController>? logger;

    public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync()
    {
        var items = (await repository.GetItemsAsync())
                    .Select(item => item.AsDto());

        logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrived {items.Count()} items");

        return items;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
        var item = await repository.GetItemAsync(id);

        if (item is null)
        {
            return NotFound();
        }

        return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Description = itemDto.Description,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await repository.CreateItemAsync(item);

        return CreatedAtAction(nameof(GetItemAsync), new {id = item.Id}, item.AsDto());
    }

    //PUT /items/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
    {
        var existingItem  = await repository.GetItemAsync(id);

        if(existingItem is null)
        {
            return NotFound();
        }

        existingItem.Name = itemDto.Name;
        existingItem.Price = itemDto.Price;

        await repository.UpdateItemAsync(existingItem);

        return NoContent();
    }

    //DELETE /Items/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItemAsync(Guid id)
    {
        var existingItem  = await repository.GetItemAsync(id);

        if(existingItem is null)
        {
            return NotFound();
        }

        await repository.DeleteItemAsync(id);

        return NoContent();
    }
}