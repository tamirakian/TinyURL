using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TinyURLApp.Models;

public class ShortenedUrlMetadata
{
    // Annotated with [BsonId] to make this property the document's primary key.
    // Annotated with [BsonRepresentation(BsonType.ObjectId)] to allow passing the parameter as type
    // string instead of an ObjectId structure. Mongo handles the conversion from string to ObjectId.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    // Annotated with [BsonElement] to represents the property name "Original" in the MongoDB collection
    [BsonElement("Original")]
    public string OriginalUrl { get; set; }

    [BsonElement("Shortened")]
    public string ShortenedUrl { get; set; }

    public DateTime _created { get; set; }

    public DateTime _updated { get; set; }
}
