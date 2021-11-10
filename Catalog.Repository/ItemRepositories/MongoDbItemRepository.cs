using Catalog.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repository.ItemRepositories
{
    public class MongoDbItemRepository : IItemRepository
    {
        private const string databaseName = "CatalogDb";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public MongoDbItemRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemCollection = database.GetCollection<Item>(collectionName);
        }

        public async Task<Item> GetItemAsync(string name)
        {

            var filter = CreateFilter(name);
            return await itemCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            var items = await itemCollection.FindAsync(new BsonDocument());
            return items.ToList();
        }

        public async Task CreateAsync(Item item)
        {

            await itemCollection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(string name, Item item)
        {
            var filter = CreateFilter(name);
            await itemCollection.ReplaceOneAsync(filter, item);
        }

        public async Task DeleteAsync(string name)
        {
            var filter = CreateFilter(name);
            await itemCollection.DeleteOneAsync(filter);
        }

        private FilterDefinition<Item> CreateFilter(string filterValue)
        {
            return filterBuilder.Eq(i => i.Name, filterValue);
        }
    }
}