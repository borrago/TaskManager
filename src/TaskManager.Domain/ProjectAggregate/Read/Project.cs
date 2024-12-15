using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.ProjectAggregate.Read;

public class Project : ReadEntity
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; }
    public IEnumerable<Task> Tasks { get; set; } = null!;
}
