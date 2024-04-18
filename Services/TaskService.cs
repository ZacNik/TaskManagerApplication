using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerApplication.Models;

namespace TaskManagerApplication.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public List<Models.Task> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }

        public Models.Task GetTaskById(int id)
        {
            return _context.Tasks.FirstOrDefault(t => t.Id == id);
        }

        public void CreateTask(Models.Task task)
        {
            task.DueDate = DateTime.Now; // Set default due date
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(Models.Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }
    }
}
