using Catalog.Entities;

namespace Catalog.Repositories;

public interface IItemsRepository
{
    Task GetItemAsync(Guid id);
    Task<IEnumerable<Item>> GetItemsAsync();
    Task CreateItemAsync(Item item);
    Task UpdateItemAsync(Item item);
    Task DeleteItemAsync(Guid id);
}
