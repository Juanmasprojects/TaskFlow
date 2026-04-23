using TaskStatus = TaskFlow.Core.TaskStatus;
using TaskItem = TaskFlow.Core.TaskItem;
using TaskFlow.Application;
using TaskFlow.Infrastructure;

var taskRepository = new FileTaskRepository("tasks.json"); // Initialize the file-based task repository
var taskService = new TaskService(taskRepository); // Create the task service with the repository

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
            filterTasks(); // Handle filtering tasks
            break;
        case "6":
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
        Console.WriteLine("5. Filter Tasks");
        Console.WriteLine("6. Exit");
        Console.Write("Select an option: ");
    }

    void createTask()
    {
        Console.WriteLine("----------------------------------------------"); // Header for create section
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
        Console.WriteLine("------------------Tasks List------------------"); // Header for task list

        var tasks = taskService.GetAllTasks(); // Get all tasks
        int index = 1; // Initialize index for task numbering
        foreach (var task in tasks)
        {
            writeLine(task, index); // Display each task
            index++; // Increment index for the next task
        }
        Console.WriteLine("----------------------------------------------"); // Footer for task list
        
        // Optionally, you can prompt the user to view task details
        viewTaskDetails(tasks);
    }

    void viewTaskDetails(IEnumerable<TaskItem> tasks)
    {
        while (true)
        {
            Console.Write("Select a task by Index or ID to view details, or press Enter to return to menu: ");
            var selection = Console.ReadLine(); // Read user selection for task details
            if (string.IsNullOrWhiteSpace(selection))
            {
                return; // Return to menu if no selection is made
            }
            var matchingTasks = searchTask(selection, tasks);
            
            if (matchingTasks.Count > 1)
            {
                Console.WriteLine("Please be more specific - multiple tasks match this ID."); // Handle multiple matches
            }
            else if (matchingTasks.Count == 1)
            {
                writeDetails(matchingTasks[0]); // Display task details
            }
            else
            {
                Console.WriteLine("Invalid selection. Please enter a valid task ID, short ID, or index."); // Handle invalid selection
            }
        }
    }



    void writeLine(TaskItem task, int index)
    {
        var indexPrefix = index == 0 ? "" : $"{index}. "; // Prefix with index if it's not 0
        Console.WriteLine($"{indexPrefix}[{task.Id.ToString().Substring(0, 6)}] {task.Title} | Status: {task.Status}");
    }

    void writeDetails(TaskItem task)
    {
        Console.WriteLine("-----------------Task Details-----------------"); // Header for task details
        Console.WriteLine($"ID: {task.Id}");
        Console.WriteLine($"Title: {task.Title}");
        Console.WriteLine($"Description: {task.Description}");
        Console.WriteLine($"Status: {task.Status}");
        Console.WriteLine($"Created At: {task.CreatedAt}");
    }
    
    List<TaskItem> searchTask(string selection, IEnumerable<TaskItem> tasks)
    {
        // Search by ID or short ID
        var matchingTasks = tasks.Where(t => t.Id.ToString().StartsWith(selection, StringComparison.OrdinalIgnoreCase)).ToList();
        
        // If no matches found by ID/short ID, try searching by index
        if (matchingTasks.Count == 0 && int.TryParse(selection, out var taskIndex) && taskIndex > 0 && taskIndex <= tasks.Count())
        {
            matchingTasks.Add(tasks.ElementAt(taskIndex - 1)); // Find task by index
        }
        
        return matchingTasks;
    }

    void updateTaskStatus()
    {
        Console.WriteLine("----------------------------------------------"); // Header for update section
        Console.Write("Enter task ID to update: ");
        var idInput = Console.ReadLine();
        var allTasks = taskService.GetAllTasks();
        var matchingTasks = searchTask(idInput ?? "", allTasks);
        
        if (matchingTasks.Count > 1)
        {
            Console.WriteLine("Please be more specific - multiple tasks match this ID."); // Handle multiple matches
        }
        else if (matchingTasks.Count == 1)
        {
            var task = matchingTasks[0];
            Console.Write("Task selected: ");
            writeLine(task, 0); // Display the selected task
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

            taskService.UpdateTaskStatus(task.Id, newStatus); // Update the task status
            Console.WriteLine("Task status updated.");
        }
        else
        {
            Console.WriteLine("Invalid selection. Please enter a valid task ID, short ID, or index."); // Handle invalid input
        }
    }

    void deleteTask()
    {
        Console.WriteLine("----------------------------------------------"); // Header for delete section
        Console.Write("Enter task ID to delete: ");
        var idInput = Console.ReadLine();
        var allTasks = taskService.GetAllTasks();
        var matchingTasks = searchTask(idInput ?? "", allTasks);
        
        if (matchingTasks.Count > 1)
        {
            Console.WriteLine("Please be more specific - multiple tasks match this ID."); // Handle multiple matches
        }
        else if (matchingTasks.Count == 1)
        {
            var task = matchingTasks[0];
            Console.Write("Task to delete: ");
            writeLine(task, 0); // Display the task
            Console.Write("Are you sure you want to delete this task? (y/n): ");
            var confirmation = Console.ReadLine();
            if (confirmation?.ToLower() == "y")
            {
                taskService.DeleteTask(task.Id); // Delete the specified task
                Console.WriteLine("Task deleted.");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }
        else
        {
            Console.WriteLine("Invalid selection. Please enter a valid task ID, short ID, or index."); // Handle invalid input
        }
    }

    void filterTasks()
    {
        Console.WriteLine("---------------Filter by status---------------"); // Header for filter section
        Console.WriteLine("1. ToDo");
        Console.WriteLine("2. InProgress");
        Console.WriteLine("3. Done");
        Console.Write("Select status to filter: ");
        var statusInput = Console.ReadLine();

        TaskStatus filterStatus = statusInput switch
        {
            "1" => TaskStatus.ToDo,
            "2" => TaskStatus.InProgress,
            "3" => TaskStatus.Done,
            _ => throw new ArgumentException("Invalid status option.")
        };

        var allTasks = taskService.GetAllTasks(); // Get all tasks
        var filteredTasks = allTasks.Where(t => t.Status == filterStatus).ToList(); // Filter by status

        if (filteredTasks.Count == 0)
        {
            Console.WriteLine("No tasks found with the selected status."); // Handle no results
        }
        else
        {
            Console.WriteLine("----------------Filtered Tasks----------------"); // Header for filtered list
            int index = 1; // Initialize index for task numbering
            foreach (var task in filteredTasks)
            {
                writeLine(task, index); // Display each filtered task
                index++; // Increment index for the next task
            }
            Console.WriteLine("----------------------------------------------"); // Footer for filtered list
            viewTaskDetails(filteredTasks); // Optionally allow viewing details of filtered tasks
        }
    }
}
