using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace TinyURLApp.Models;

public class UrlMetadata
{
    // Annotated with [BsonId] to make this property the document's primary key.
    // Annotated with [BsonRepresentation(BsonType.ObjectId)] to allow passing the parameter as type
    // string instead of an ObjectId structure. Mongo handles the conversion from string to ObjectId.
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    // Annotated with [BsonElement] to represents the property name "Original" in the MongoDB collection
    [BsonElement("Original")]
    // Annotated with [JsonPropertyName] to represents the property name in the web API's serialized JSON response.
    [JsonPropertyName("Original")]
    public string OriginalUrl { get; set; }
    [BsonElement("Short")]
    [JsonPropertyName("Short")]
    public string ShortUrl { get; set; }
}
