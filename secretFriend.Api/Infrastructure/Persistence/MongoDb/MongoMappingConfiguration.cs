using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace secretFriend.Api.Infrastructure.Persistence.MongoDb;

public static class MongoMappingConfiguration
{
    private const string DEFAULT_CONVENTION_NAME = "default_Convention";

    public static void Configure()
    {
        ConfigureConventions();
    }

    private static void ConfigureConventions()
    {
        var conventionPack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreExtraElementsConvention(true),
            new IgnoreIfNullConvention(true),
            new EnumRepresentationConvention(BsonType.String)
        };

        ConventionRegistry.Register(DEFAULT_CONVENTION_NAME, conventionPack, t => true);
    }
}