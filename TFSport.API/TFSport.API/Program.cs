using Microsoft.Azure.Cosmos;
using TFSport.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<TFSport.Services.Interfaces.IUserService,TFSport.Services.Services.UserService>();
builder.Services.AddScoped<TFSport.Services.Interfaces.IEmailService,TFSport.Services.Services.EmailService>();
builder.Services.AddAutoMapper(typeof(AutoUserMapper));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(sp =>
{
	string connectionString = builder.Configuration.GetConnectionString("CosmosDb");
	return new CosmosClient(connectionString);
});

builder.Services.AddCosmosRepository(options =>
{
    var settings = builder.Configuration.GetSection("CosmosConfiguration");
    options.CosmosConnectionString = builder.Configuration.GetConnectionString("CosmosDb");

	options.DatabaseId = settings.GetSection("DatabaseId").Value;
    options.ContainerPerItemType = true;

    options.ContainerBuilder
        .Configure<TFSport.Models.User>(containerOptionsBuilder =>
        {
            containerOptionsBuilder
                .WithContainer("Users")
                .WithPartitionKey("/partitionKey");
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
