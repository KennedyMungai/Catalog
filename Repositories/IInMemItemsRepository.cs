using Catalog.Entities;

namespace Catalog.Repositories;

public interface IInMemItemsRepository
{
    Item GetItem(Guid id);
    IEnumerable<Item> GetItems();
}
