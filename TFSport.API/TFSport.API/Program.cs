using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using TFSport.API.AutoMapper;
using TFSport.API.Filters;
using TFSport.Services.Interfaces;
using TFSport.Services.Services;
using TFSport.Models.Entities;
using TFSport.Repository.Interfaces;
using TFSport.Repository.Repositories;
using TFSport.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
builder.Services.Configure<BlobStorageOptions>(builder.Configuration.GetSection(nameof(BlobStorageOptions)));
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IArticlesRepository, ArticlesRepository>();
builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();
builder.Services.AddScoped<IAuthorStatisticsRepository, AuthorStatisticsRepository>();
builder.Services.AddScoped<ITagsRepository, TagsRepository>();
builder.Services.AddScoped<IFavoritesService, FavoritesService>();
builder.Services.AddScoped<ISportsService, SportsService>();
builder.Services.AddScoped<ITopAuthorsService, TopAuthorsService>();
builder.Services.AddScoped<ILikesService, LikesService>();
builder.Services.AddScoped<CustomExceptionFilter>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var allowedOrigins = builder.Configuration.GetSection("CORS:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins(allowedOrigins)
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

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

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["ConnectionStrings:BlobStorage"]);
});

builder.Services.AddBlobStorageService(builder.Configuration["ConnectionStrings:BlobStorage"]);

builder.Services.AddSwaggerGen(options =>
{
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));

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

builder.Services.AddSingleton(sp =>
{
    string connectionString = builder.Configuration.GetConnectionString("CosmosDb");
    return new CosmosClient(connectionString);
});

builder.Services.AddCosmosRepository(options =>
{
    var cosmosConfiguration = builder.Configuration.GetSection("CosmosConfiguration");
    string databaseId;

    if (builder.Environment.IsDevelopment())
    {
        databaseId = cosmosConfiguration.GetValue<string>("DevDatabaseId");
    }
    else
    {
        databaseId = cosmosConfiguration.GetValue<string>("QaDatabaseId");
    }

    options.CosmosConnectionString = builder.Configuration.GetConnectionString("CosmosDb");
    options.DatabaseId = databaseId;
    options.ContainerPerItemType = true;

    options.ContainerBuilder
        .Configure<TFSport.Models.Entities.User>(containerOptionsBuilder =>
        {
            containerOptionsBuilder
                .WithContainer("Users")
                .WithPartitionKey("/partitionKey");
        });
	options.ContainerBuilder
		.Configure<Article>(containerOptionsBuilder =>
		{
			containerOptionsBuilder
				.WithContainer("Articles")
				.WithPartitionKey("/partitionKey");
		});
	options.ContainerBuilder
		.Configure<Comment>(containerOptionsBuilder =>
		{
			containerOptionsBuilder
				.WithContainer("Comments")
				.WithPartitionKey("/articleId");
		});
	options.ContainerBuilder
		.Configure<Favorites>(containerOptionsBuilder =>
		{
			containerOptionsBuilder
				.WithContainer("Users")
				.WithPartitionKey("/partitionKey");
		});
    options.ContainerBuilder
        .Configure<Tag>(containerOptionsBuilder =>
        {
            containerOptionsBuilder
                .WithContainer("Articles")
                .WithPartitionKey("/partitionKey");
        });
    options.ContainerBuilder
        .Configure<AuthorStatistics>(containerOptionsBuilder =>
        {
            containerOptionsBuilder
                .WithContainer("Users")
                .WithPartitionKey("/partitionKey");
        });
});

builder.Logging.AddApplicationInsights(
		configureTelemetryConfiguration: (config) =>
			config.ConnectionString = builder.Configuration.GetConnectionString("AppInsights"),
			configureApplicationInsightsLoggerOptions: (options) => { }
	);

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("Successful-operations", LogLevel.Information);
builder.Services.AddMemoryCache(options =>
{
    options.CompactionPercentage = 0.02;
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(3);
    options.SizeLimit = 1000;
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "text/plain";

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is Exception error)
        {
            await context.Response.WriteAsync(error.Message).ConfigureAwait(false);
        }
    });
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
	userService.CreateSuperAdminUser().Wait();
}

app.Run();