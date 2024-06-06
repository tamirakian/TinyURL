using TinyURLApp.Data.Repositories;
using TinyURLApp.Models;
using TinyURLApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TinyUrlDatabaseSettings>(
    builder.Configuration.GetSection("TinyUrlDatabase"));

builder.Services.AddSingleton<ITinyUrlDatabaseRepository, TinyUrlDatabaseRepository>();
builder.Services.AddSingleton<IUrlShorteningService, UrlShorteningService>();
builder.Services.AddSingleton<ICacheService<string, string>>(new LruCacheService<string, string>(5));

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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
