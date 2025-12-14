namespace secretFriend.Api.Infrastructure.Configuration;

public sealed record MongoSettings
{
    public string ConnectionString { get; init; } = string.Empty;
    public string DatabaseName { get; init; } = string.Empty;
}