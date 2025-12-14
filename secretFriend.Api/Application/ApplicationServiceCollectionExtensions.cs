using secretFriend.Api.Application.Interfaces;
using secretFriend.Api.Application.Services;

namespace secretFriend.Api.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<Random>();
        services.AddScoped<ISecretFriendService, SecretFriendService>();
        services.AddScoped<IWishlistService, WishlistService>();
        
        return services;
    }
}