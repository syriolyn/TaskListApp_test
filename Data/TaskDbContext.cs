using Microsoft.EntityFrameworkCore;
using TaskListApp.Models;

namespace TaskListApp.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) 
            : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>().ToTable("Tasks");
            
            // Начальные данные для базы
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem { Id = 1, Title = "Изучить ASP.NET Core", 
                               Description = "Освоить основы ASP.NET Core MVC", 
                               IsCompleted = false },
                new TaskItem { Id = 2, Title = "Написать приложение", 
                               Description = "Создать приложение списка задач", 
                               IsCompleted = true }
            );
        }
    }
}