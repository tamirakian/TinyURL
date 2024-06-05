using TinyURLApp.Models;
using TinyURLApp.Services;

var builder = WebApplication.CreateBuilder(args);

/*
Add services to the container.
  
The configuration instance to which the appsettings.json
file's TinyUrlDatabase section binds is registered in the Dependency Injection (DI) container.

DI - a tool or framework that manages the dependencies between different components of a software application.

For example, the TinyUrlDatabaseSettings object's ConnectionString property is populated
with the TinyUrlDatabase:ConnectionString property in appsettings.json.
*/
builder.Services.Configure<TinyUrlDatabaseSettings>(
    builder.Configuration.GetSection("TinyUrlDatabase"));

// The UrlShorteningService class is registered with DI to support constructor injection in consuming classes.
// The singleton service lifetime is most appropriate because UrlShorteningService takes a direct dependency on MongoClient.
// A singleton service means that the DI container will create only one instance of the service throughout the application's lifetime.
// This single instance will be reused every time the service is requested.
builder.Services.AddSingleton<UrlShorteningService>();

// "AddJsonOptions" 
// property names in the web API's serialized JSON response match their corresponding property names in the CLR object type.
// For example, the "ShortenedUrlMetadata" class's "OriginalUrl" property serializes as "OriginalUrl" instead of "originalUrl".
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
