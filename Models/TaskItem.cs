using System.Text.Json.Serialization;
using TaskTrackerCli.Enums;

namespace TaskTrackerCli.Models;

public sealed class TaskItem
{
    [JsonInclude]
    public int Id { get; private set; }

    [JsonInclude]
    public string Description { get; private set; } = string.Empty;

    [JsonInclude]
    public Status Status { get; private set; }

    [JsonInclude]
    public DateTime CreatedAt { get; private set; }

    [JsonInclude]
    public DateTime? UpdatedAt { get; private set; }

    [JsonConstructor]
    private TaskItem() { }

    private TaskItem(int id, string description)
    {
        Id = id;
        Description = description;
        Status = Status.Todo;
        CreatedAt = DateTime.UtcNow;
    }

    public static TaskItem Create(int id, string description) => new(id, description);

    public bool Update(string description)
    {
        Description = description;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    public bool UpdateStatus(Status status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }
}
