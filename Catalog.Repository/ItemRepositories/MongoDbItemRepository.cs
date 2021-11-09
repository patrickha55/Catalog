using Catalog.Data.Entities;
using MongoDB.Driver;

namespace Catalog.Repository.ItemRepositories
{
    public class MongoDbItemRepository : IItemRepository
    {
        private const string databaseName = "CatalogDb";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> items;

        public MongoDbItemRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            items = database.GetCollection<Item>(collectionName);
        }

        public Task<Item> GetItemAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Item>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(Item item)
        {

            await items.InsertOneAsync(item);
        }

        public Task UpdateAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}