using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(sp =>
{
	string connectionString = builder.Configuration.GetConnectionString("CosmosDb");
	return new CosmosClient(connectionString);
});

builder.Services.AddCosmosRepository(options =>
{
    var settings = builder.Configuration.GetSection("CosmosConfiguration");

    options.DatabaseId = settings.GetSection("DatabaseId").Value;
    options.ContainerPerItemType = true;

    options.ContainerBuilder
        .Configure<TFSport.Models.User>(containerOptionsBuilder =>
        {
            containerOptionsBuilder
                .WithContainer("Users")
                .WithPartitionKey("PartitionKey");
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
