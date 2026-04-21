// Repository that saves and loads tasks from a JSON file
using System.Text.Json;
using TaskFlow.Application;
using TaskFlow.Core;

namespace TaskFlow.Infrastructure
{
    public class FileTaskRepository : ITaskRepository
    {
        private readonly string _filePath;

        public FileTaskRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<TaskItem> GetAllTasks()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TaskItem>();
            }

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }

        public void SaveAll(List<TaskItem> tasks)
        {
            var json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}