using Microsoft.Extensions.Options;
using MongoDB.Driver;
using secretFriend.Api.Infrastructure.Configuration;
using secretFriend.Api.Infrastructure.Persistence.MongoDb;
using secretFriend.Api.Domain.Repositories;

namespace secretFriend.Api.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMongoDb(configuration);
        services.AddRepositories();
        
        return services;
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection("MongoDB"));

        services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
            var client = new MongoClient(settings.ConnectionString);
            return client.GetDatabase(settings.DatabaseName);
        });

        MongoMappingConfiguration.Configure();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISecretFriendGameRepository, SecretFriendGameRepository>();
        
        return services;
    }
}