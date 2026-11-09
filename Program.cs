using System.Drawing;
using TaskTrackerCli.Enums;
using TaskTrackerCli.Services;

Console.ForegroundColor = ConsoleColor.Yellow;

Console.WriteLine("Welcome to task cli, to get type help to get commands, type stop to quit");
Console.ResetColor();
while (true)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write("task-cli ");
    Console.ResetColor();
    var command = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(command))
        continue;

    if (command.Equals("stop", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Stopping application");
        break;
    }
    if (command.Equals("help", StringComparison.OrdinalIgnoreCase))
    {
        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine("############### task cli menu #############");
        Console.WriteLine("add \"some task\"");
        Console.WriteLine("update id \"some task\"");
        Console.WriteLine("delete id");
        Console.WriteLine("mark-in-progress id");
        Console.WriteLine("mark-done id");
        Console.WriteLine("list");
        Console.WriteLine("list todo");
        Console.WriteLine("list in-progress");
        Console.WriteLine("list done");
        Console.WriteLine("stop");
        Console.WriteLine("###########################################");
        Console.ResetColor();
        continue;
    }
    if (command.StartsWith("add", StringComparison.OrdinalIgnoreCase))
    {
        int spaceIndex = command.IndexOf(' ');
        if (spaceIndex == -1)
        {
            Console.WriteLine("Invalid command, must write add \"the task\"");
            continue;
        }

        var taskArg = command.Substring(spaceIndex + 1).Trim();

        if (!taskArg.StartsWith('\"') || !taskArg.EndsWith('\"') || taskArg.Length < 2)
        {
            Console.WriteLine("Invalid command, must write add \"the task\"");
            continue;
        }

        var taskDescription = taskArg.Substring(1, taskArg.Length - 2).Trim();

        var success = TaskService.AddTask(taskDescription);
        if (success)
        {
            Console.WriteLine("Task Added Successfully!");
        }
        else
        {
            Console.WriteLine("Task Not Added!");
        }
        continue;
    }

    if (command.StartsWith("update", StringComparison.OrdinalIgnoreCase))
    {
        // 1. Find the space after "update"
        int spaceIndex = command.IndexOf(' ');
        if (spaceIndex == -1)
        {
            Console.WriteLine("Invalid command, must write update ID \"new task description\"");
            continue;
        }

        // 2. Get everything AFTER "update" (e.g., "1 \"some task\"")
        var arguments = command.Substring(spaceIndex + 1).Trim();

        // 3. Find the space separating the ID from the quoted task description
        int argSpaceIndex = arguments.IndexOf(' ');
        if (argSpaceIndex == -1)
        {
            Console.WriteLine("Invalid command, must write update ID \"new task description\"");
            continue;
        }

        // 4. Extract ID and the task argument
        string idString = arguments.Substring(0, argSpaceIndex);
        var taskArg = arguments.Substring(argSpaceIndex + 1).Trim();

        // 5. Parse and validate the ID
        if (!int.TryParse(idString, out int taskId))
        {
            Console.WriteLine("Invalid task ID. Must be a number.");
            continue;
        }

        // 6. Validate that the task description is properly quoted
        if (!taskArg.StartsWith('\"') || !taskArg.EndsWith('\"') || taskArg.Length < 2)
        {
            Console.WriteLine("Invalid command, must write update ID \"new task description\"");
            continue;
        }

        // 7. Remove quotes and execute update
        var newDescription = taskArg.Substring(1, taskArg.Length - 2).Trim();

        var success = TaskService.UpdateTask(taskId, newDescription);
        if (success)
        {
            Console.WriteLine("Task Updated Successfully!");
        }
        else
        {
            Console.WriteLine("Task Not Updated!");
        }
        continue;
    }

    if (command.StartsWith("delete", StringComparison.OrdinalIgnoreCase))
    {
        int spaceIndex = command.IndexOf(' ');
        if (spaceIndex == -1)
        {
            Console.WriteLine("Invalid command, must write delete ID");
            continue;
        }

        string idString = command.Substring(spaceIndex + 1).Trim();

        if (!int.TryParse(idString, out int taskId))
        {
            Console.WriteLine("Invalid task ID. Must be a number.");
            continue;
        }

        var success = TaskService.DeleteTask(taskId);

        if (success)
        {
            Console.WriteLine("Task Deleted Successfully!");
        }
        else
        {
            Console.WriteLine("Task Not Deleted!");
        }
        continue;
    }

    if (command.StartsWith("mark-in-progress", StringComparison.OrdinalIgnoreCase))
    {
        int spaceIndex = command.IndexOf(' ');
        if (spaceIndex == -1)
        {
            Console.WriteLine("Invalid command, must write mark-in-progress ID");
            continue;
        }

        string idString = command.Substring(spaceIndex + 1).Trim();

        if (!int.TryParse(idString, out int taskId))
        {
            Console.WriteLine("Invalid task ID. Must be a number.");
            continue;
        }

        var success = TaskService.UpdateTaskStatus(taskId, Status.InProgress);

        if (success)
        {
            Console.WriteLine("Task Marked In-Progress Successfully!");
        }
        else
        {
            Console.WriteLine("Task Not Marked In-Progress!");
        }
        continue;
    }

    if (command.StartsWith("mark-done", StringComparison.OrdinalIgnoreCase))
    {
        int spaceIndex = command.IndexOf(' ');
        if (spaceIndex == -1)
        {
            Console.WriteLine("Invalid command, must write mark-done ID");
            continue;
        }

        string idString = command.Substring(spaceIndex + 1).Trim();

        if (!int.TryParse(idString, out int taskId))
        {
            Console.WriteLine("Invalid task ID. Must be a number.");
            continue;
        }

        var success = TaskService.UpdateTaskStatus(taskId, Status.Done);

        if (success)
        {
            Console.WriteLine("Task Marked Done Successfully!");
        }
        else
        {
            Console.WriteLine("Task Not Marked Done!");
        }
        continue;
    }
    if (command.StartsWith("list", StringComparison.OrdinalIgnoreCase))
    {
        int spaceIndex = command.IndexOf(' ');
        Status? statusFilter = null;

        // If there's an argument after "list" (e.g., "list done")
        if (spaceIndex != -1)
        {
            var statusArg = command.Substring(spaceIndex + 1).Trim().ToLowerInvariant();

            // Map string argument to your Status enum
            statusFilter = statusArg switch
            {
                "done" => Status.Done,
                "todo" => Status.Todo,
                "in-progress" => Status.InProgress,
                _ => null, // Handle invalid status or clear it out
            };

            if (statusFilter == null && !string.IsNullOrEmpty(statusArg))
            {
                Console.WriteLine("Invalid status. Use: done, todo, or in-progress");
                continue;
            }
        }

        // Call your service method
        var tasks = TaskService.ListTasks(statusFilter);

        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found.");
        }
        else
        {
            Console.WriteLine($"\n--- Task List ({statusFilter?.ToString() ?? "All"}) ---");
            foreach (var task in tasks)
            {
                Console.WriteLine($"ID: {task.Id} | [{task.Status}] {task.Description}");
            }
            Console.WriteLine();
        }

        continue;
    }
    Console.WriteLine($"Unknown command: {command}");
}
