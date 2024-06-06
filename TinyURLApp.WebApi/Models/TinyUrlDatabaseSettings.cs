namespace TinyURLApp.Models;

// Used to store the appsettings.json file's TinyUrlDatabase property values.
public class TinyUrlDatabaseSettings
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string UrlsMetadataCollectionName { get; set; }
}
