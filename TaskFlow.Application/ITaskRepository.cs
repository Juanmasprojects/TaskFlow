//create public Interface to save a list of tasks in the database
using TaskFlow.Core;

namespace TaskFlow.Application

{
    public interface ITaskRepository
    {
       List<TaskItem> GetAllTasks();
       void SaveAll(List<TaskItem> tasks);   
       
    }
}