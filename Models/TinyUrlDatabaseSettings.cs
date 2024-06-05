namespace TinyURLApp.Models;

// Used to store the appsettings.json file's TinyUrlDatabase property values.
// The JSON and C# property names are named identically to ease the mapping process.
public class TinyUrlDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string ShortenedUrlsMetadataCollectionName { get; set; }
}
