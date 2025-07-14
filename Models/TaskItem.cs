using System;
using System.ComponentModel.DataAnnotations;

namespace TaskListApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название задачи обязательно")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 100 символов")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string Description { get; set; }

        [Display(Name = "Выполнено")]
        public bool IsCompleted { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}