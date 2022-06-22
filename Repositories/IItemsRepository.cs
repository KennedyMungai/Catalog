using Catalog.Entities;

namespace Catalog.Repositories;

public interface IItemsRepository
{
    Item GetItemAsync(Guid id);
    IEnumerable<Item> GetItemsAsync();
    void CreateItemAsync(Item item);
    void UpdateItemAsync(Item item);
    void DeleteItemAsync(Guid id);
}
