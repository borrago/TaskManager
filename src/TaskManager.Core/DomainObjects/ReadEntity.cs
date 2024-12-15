using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskManager.Core.DomainObjects;

public abstract class ReadEntity : IReadEntity
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
}
