using System.Text.Json;
using TaskTrackerCli.Enums;
using TaskTrackerCli.Models;

namespace TaskTrackerCli.Services;

public class TaskService
{
    private const string filePath = $"tasks.json";

    private static readonly List<TaskItem> taskItems = LoadTasks();

    // Dynamically set Identity based on existing tasks to avoid ID collisions
    private static int Identity = taskItems.Count > 0 ? taskItems.Max(t => t.Id) + 1 : 1;

    private static List<TaskItem> LoadTasks()
    {
        if (!File.Exists(filePath))
        {
            return [];
        }

        try
        {
            string json = File.ReadAllText(filePath);

            // Handle empty or whitespace files gracefully
            if (string.IsNullOrWhiteSpace(json))
            {
                return [];
            }

            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? [];
        }
        catch (Exception)
        {
            // If the file is corrupted, fallback to an empty list
            return [];
        }
    }

    public static int GenerateId()
    {
        return Identity++;
    }

    public static bool AddTask(string description)
    {
        var task = TaskItem.Create(GenerateId(), description);
        taskItems.Add(task);

        try
        {
            var json = JsonSerializer.Serialize(
                taskItems,
                new JsonSerializerOptions { WriteIndented = true }
            );
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            if (Identity > 1)
                Identity--;

            taskItems.Remove(task); // Rollback in-memory list as well
            Console.WriteLine(ex.Message);
            return false;
        }

        return true;
    }

    public static bool UpdateTask(int id, string description)
    {
        var task = taskItems.FirstOrDefault(t => t.Id == id);
        if (task is null)
        {
            Console.WriteLine($"Task with id: {id} not found");
            return false;
        }
        task.Update(description);
        try
        {
            var json = JsonSerializer.Serialize(
                taskItems,
                new JsonSerializerOptions { WriteIndented = true }
            );
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        return true;
    }

    public static bool DeleteTask(int id)
    {
        var task = taskItems.FirstOrDefault(t => t.Id == id);
        if (task is null)
        {
            Console.WriteLine($"Task with id: {id} not found");
            return false;
        }
        taskItems.Remove(task);
        try
        {
            var json = JsonSerializer.Serialize(
                taskItems,
                new JsonSerializerOptions { WriteIndented = true }
            );
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        return true;
    }

    public static bool UpdateTaskStatus(int taskId, Status status)
    {
        var task = taskItems.FirstOrDefault(t => t.Id == taskId);
        if (task is null)
        {
            Console.WriteLine($"Task with id: {taskId} not found");
            return false;
        }
        task.UpdateStatus(status);
        try
        {
            var json = JsonSerializer.Serialize(
                taskItems,
                new JsonSerializerOptions { WriteIndented = true }
            );
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        Console.WriteLine($"Task  with id: {taskId} updated successfully");
        return true;
    }

    public static List<TaskItem> ListTasks(Status? status)
    {
        // 1. Check if the file exists; if not, return an empty list
        if (!File.Exists(filePath))
        {
            return [];
        }

        // 2. Read the actual text content from the file
        var jsonContent = File.ReadAllText(filePath);

        // 3. Handle empty files gracefully
        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            return [];
        }

        // 4. Deserialize the JSON string content, not the path
        var tasks = JsonSerializer.Deserialize<List<TaskItem>>(jsonContent) ?? [];

        return status is null ? tasks : [.. tasks.Where(t => t.Status == status)];
    }
}
