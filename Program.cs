using System.Data.Common;
using Catalog.Repositories;
using Catalog.Settings;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => 
    {
        options.SuppressAsyncSuffixInActionNames = false;
    }
);

var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
builder.Services.Configure<MongoDbItemsRepository>(mongoDbSettings);
builder.Services.AddSingleton<IItemsRepository, MongoDbItemsRepository>();
builder.Services.AddHealthChecks()
    .AddMongoDb(
        mongoDbSettings.GetConnectionString("MongoDbSettings"), 
        name: "mongodb", timeout: 
        TimeSpan.FromSeconds(3),
        tags: new[]{"ready"}
        );

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
app.MapHealthChecks("/health");

app.Run();
