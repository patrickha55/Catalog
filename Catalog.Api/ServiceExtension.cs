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
        public static MongoDbSettings mongoDbSettings;
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Tell mongodb to serialize Guid and datetimeOffset as string
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                return new MongoClient(mongoDbSettings.ConnectionString);
            });

            services.AddSingleton<IItemRepository, MongoDbItemRepository>();
        }

        public static void ConfigureHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks()
                    .AddMongoDb(
                        mongoDbSettings.ConnectionString,
                        name: "mongodb",
                        timeout: TimeSpan.FromSeconds(3),
                        tags: new[] { "ready" });
        }
    }
}