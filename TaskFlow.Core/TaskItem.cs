//Represents a task with Id, Title, Description, Status and CreatedAt
using System;

namespace TaskFlow.Core
{
    public class TaskItem
    {
        public Guid Id { get; set; } // Unique identifier for the task
        public string Title { get; set; } = string.Empty; // Title of the task
        public string Description { get; set; } = string.Empty; // Detailed description of the task
        public TaskStatus Status { get; set; } // Current status of the task (ToDo, InProgress, Done)
        public DateTime CreatedAt { get; set; } // Timestamp when the task was created

        public TaskItem()
        {
            Id = Guid.NewGuid(); // Automatically generate a unique Id
            CreatedAt = DateTime.UtcNow; // Set creation time to now
            Status = TaskStatus.ToDo; // Default status is ToDo
        }

        public void MarkInProgress()
        {
            Status = TaskStatus.InProgress; // Change status to InProgress
        }

        public void MarkDone()
        {
            Status = TaskStatus.Done; // Change status to Done
        }

    }

}
