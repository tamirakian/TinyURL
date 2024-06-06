using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TinyURLApp.Models;

public class UrlMetadata
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("Original")]
    [JsonPropertyName("Original")]
    public string OriginalUrl { get; set; }

    [BsonElement("Short")]
    [JsonPropertyName("Short")]
    public string ShortUrl { get; set; }
}
