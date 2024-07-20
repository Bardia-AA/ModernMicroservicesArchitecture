using MongoDB.Driver;
using Shared.DTOs;

namespace GrpcService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
            _database = client.GetDatabase("GrpcServiceDb");
        }

        public IMongoCollection<ExampleDto> ExampleDtos => _database.GetCollection<ExampleDto>("ExampleDtos");
    }
}