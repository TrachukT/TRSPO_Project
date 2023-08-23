using Microsoft.Extensions.DependencyInjection;
using TFSport.Models;

namespace TFSport.Services.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlobStorageService(this IServiceCollection services, string connectionString)
        {
            services.Configure<BlobStorageOptions>(options => options.ConnectionString = connectionString);

            return services;
        }
    }
}
