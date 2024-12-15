using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using TaskManager.Core.DomainObjects;

namespace TaskManager.Domain.ProjectAggregate.Read;

public class Task : ReadEntity
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = "";
    public string Priority { get; set; } = "";

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}
