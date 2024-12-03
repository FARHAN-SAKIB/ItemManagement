using MongoDB.Driver;
using ItemManagementAPI.Models;
using Microsoft.Extensions.Options;

namespace ItemManagementAPI.Services
{
    public class ItemService
    {
        private readonly IMongoCollection<Item> _items;

        public ItemService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _items = database.GetCollection<Item>(settings.Value.ItemsCollectionName);
        }

        public List<Item> Get() => _items.Find(item => true).ToList();

        public Item Get(string id) => _items.Find<Item>(item => item.Id == id).FirstOrDefault();


        public Item Create(Item item)
        {
          
                item.Id = Guid.NewGuid().ToString();
                _items.InsertOne(item);
            return item;
        }


        public void Update(string id, Item updatedItem) => _items.ReplaceOne(item => item.Id == id, updatedItem);

        public void Remove(string id) => _items.DeleteOne(item => item.Id == id);
    }
}