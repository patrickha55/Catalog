using Catalog.Data.Entities;

namespace Catalog.Repository.ItemRepositories
{
    public interface IItemRepository
    {
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<Item> GetItemAsync(string name);
        Task CreateAsync(Item request);
        Task UpdateAsync(string name, Item request);
        Task DeleteAsync(string name);
    }
}