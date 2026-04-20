using TaskStatus = TaskFlow.Core.TaskStatus;
using TaskFlow.Application;
using TaskFlow.Core;

var taskService = new TaskService(); // Create an instance of TaskService to manage tasks

bool running = true; // Flag to control the main loop

while (running)
{
    showMenu(); // Display the menu options

    var input = Console.ReadLine(); // Read user input

    switch (input)
    {
        case "1":
            createTask(); // Handle task creation
            break;
        case "2":
            listTasks(); // Handle listing all tasks
            break;
        case "3":
            updateTaskStatus(); // Handle updating task status
            break;
        case "4":
            deleteTask(); // Handle deleting a task
            break;
        case "5":
            running = false; // Exit the application
            break;
        default:
            Console.WriteLine("Invalid option. Please try again."); // Handle invalid input
            break;
    }

    void showMenu()
    {
        Console.WriteLine("TaskFlow - Task Management");
        Console.WriteLine("1. Create Task");
        Console.WriteLine("2. List Tasks");
        Console.WriteLine("3. Update Task Status");
        Console.WriteLine("4. Delete Task");
        Console.WriteLine("5. Exit");
        Console.Write("Select an option: ");
    }

    void createTask()
    {
        Console.Write("Enter task title: ");
        var title = Console.ReadLine() ?? ""; //Read task title from user

        Console.Write("Enter task description: ");
        var description = Console.ReadLine() ?? ""; //ead task description from user

        try
        {
            var task = taskService.CreateTask(title, description); // Create a new task
            Console.WriteLine($"Task created with ID: {task.Id}"); // Display the created task ID
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message); // Handle validation errors
        }
    }

    void listTasks()
    {
        var tasks = taskService.GetAllTasks(); // Get all tasks
        foreach (var task in tasks)
        {
            Console.WriteLine($"[{task.Status}] {task.Title} | ID: {task.Id} | Created: {task.CreatedAt:g}");
        }
    }

    void updateTaskStatus()
    {
        Console.Write("Enter task ID to update: ");
        var idInput = Console.ReadLine();
        if (Guid.TryParse(idInput, out var taskId))
        {
            Console.WriteLine("Select new status:");
            Console.WriteLine("1. ToDo");
            Console.WriteLine("2. InProgress");
            Console.WriteLine("3. Done");
            var statusInput = Console.ReadLine();

            TaskStatus newStatus = statusInput switch
            {
                "1" => TaskStatus.ToDo,
                "2" => TaskStatus.InProgress,
                "3" => TaskStatus.Done,
                _ => throw new ArgumentException("Invalid status option.")
            };

            taskService.UpdateTaskStatus(taskId, newStatus); // Update the task status
            Console.WriteLine("Task status updated.");
        }
        else
        {
            Console.WriteLine("Invalid task ID format."); // Handle invalid GUID input
        }
    }

    void deleteTask()
    {
        Console.Write("Enter task ID to delete: ");
        var idInput = Console.ReadLine();
        if (Guid.TryParse(idInput, out var taskId))
        {
            taskService.DeleteTask(taskId); // Delete the specified task
            Console.WriteLine("Task deleted.");
        }
        else
        {
            Console.WriteLine("Invalid task ID format."); // Handle invalid GUID input
        }
    }
}
