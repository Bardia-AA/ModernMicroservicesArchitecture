#if MONGODB
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
#endif

namespace Shared.DTOs
{
    public class ExampleDto
    {
#if MONGODB
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
#endif
        public string Id { get; set; }

#if MONGODB
        [BsonElement("name")]
#endif
        public string Name { get; set; }
    }
}