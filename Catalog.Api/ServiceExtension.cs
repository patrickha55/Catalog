using Catalog.Api.Settings;
using Catalog.Repository.ItemRepositories;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Catalog.Api
{
    public static class ServiceExtension
    {
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Tell mongodb to serialize Guid and datetimeOffset as string
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var settings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
                return new MongoClient(settings.ConnectionString);
            });

            services.AddSingleton<IItemRepository, MongoDbItemRepository>();
        }
    }
}