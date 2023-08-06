using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Azure.Cosmos;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TFSport.Services;
using TFSport.Services.Interfaces;
using TFSport.API.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("JWT:Secret").Value)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            RequireExpirationTime = true,
            ValidateLifetime = true
        };
        options.SaveToken = true;
    });

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(RoleAuthorizationFilter));
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TFSport.API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Standard Authorization header using the Bearer scheme. Example: (\"Bearer {token}\")"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            },
            new List<string>()
        }
    });

    options.EnableAnnotations();
});

builder.Services.AddEndpointsApiExplorer();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
