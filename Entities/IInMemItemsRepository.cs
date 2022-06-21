namespace Catalog.Entities;

public interface IInMemItemsRepository
{
    Item GetItem(Guid id);
    IEnumerable<Item> GetItems();
}
