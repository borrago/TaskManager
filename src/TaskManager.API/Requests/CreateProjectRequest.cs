namespace TaskManager.API.Requests;

public class CreateProjectRequest
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public Guid UserId { get; set; }
}
