// Service to manage tasks: create, get all, update status and delete tasks
using TaskStatus = TaskFlow.Core.TaskStatus;
using System;
using System.Linq;
using TaskFlow.Core;

namespace TaskFlow.Application
{
    public class TaskService
    {
        private readonly List<TaskItem> _tasks = new List<TaskItem>(); // In-memory list to store tasks
        private readonly ITaskRepository _taskRepository; // Repository to persist tasks

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository; // Inject the task repository
            _tasks = _taskRepository.GetAllTasks(); //  Load existing tasks from the repository
        }

        public TaskItem CreateTask(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty."); // Validate title
            }
        
            var task = new TaskItem
            {
                Title = title,
                Description = description
            };
            _tasks.Add(task); // Add new task to the list
            _taskRepository.SaveAll(_tasks); // Save the updated list to the repository
            return task; // Return the created task
        }
        public IEnumerable<TaskItem> GetAllTasks()
        {
            return _tasks; // Return all tasks
        }

        public void UpdateTaskStatus(Guid taskId, TaskStatus status)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId); // Find the task by Id
            if (task != null)
            {
                switch (status)
                {
                    case TaskStatus.ToDo:
                        task.Status = TaskStatus.ToDo; // Update status to ToDo
                        break;
                    case TaskStatus.InProgress:
                        task.MarkInProgress(); // Update status to InProgress
                        break;
                    case TaskStatus.Done:
                        task.MarkDone(); // Update status to Done
                        break;
                }
            }
            _taskRepository.SaveAll(_tasks); // Save the updated list to the repository
        }
        public void DeleteTask(Guid taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId); // Find the task by Id
            if (task != null)
            {
                _tasks.Remove(task); // Remove the task from the list
                _taskRepository.SaveAll(_tasks); // Save the updated list to the repository
            }
        }
    }
}