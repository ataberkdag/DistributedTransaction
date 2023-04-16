using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Report.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class ReportItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("correlationId")]
        public Guid CorrelationId { get; set; }

        [BsonElement("request")]
        public string Request { get; set; }
    }
}
