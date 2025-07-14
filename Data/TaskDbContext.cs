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
    modelBuilder.Entity<TaskItem>().HasData(
        new TaskItem 
        { 
            Id = 1, 
            Title = "Изучить ASP.NET Core",
            Description = "Освоить основы ASP.NET Core MVC",
            IsCompleted = false,
            CreatedDate = new DateTime(2023, 1, 1) // Фиксированная дата вместо DateTime.Now
        },
        new TaskItem 
        { 
            Id = 2, 
            Title = "Написать приложение",
            Description = "Создать приложение списка задач",
            IsCompleted = true,
            CreatedDate = new DateTime(2023, 1, 2) // Фиксированная дата
        }
    );
}
    }
}