using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskManager.Core.DomainObjects;

public interface IReadEntity
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    Guid Id { get; set; }
}
