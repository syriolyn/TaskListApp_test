using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskListApp.Data;
using TaskListApp.Models;

namespace TaskListApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly TaskDbContext _context;

        // Внедрение зависимости контекста базы данных
        public TasksController(TaskDbContext context)
        {
            _context = context;
        }

        // GET: Tasks - отображение списка с фильтрацией
        public async Task<IActionResult> Index(string searchString, bool? showCompleted)
        {
            // Базовый запрос
            var tasks = _context.Tasks.AsQueryable();

            // Фильтрация по поисковой строке
            if (!string.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(t => t.Title.Contains(searchString) || 
                                      t.Description.Contains(searchString));
            }

            // Фильтрация по статусу выполнения
            if (showCompleted.HasValue)
            {
                tasks = tasks.Where(t => t.IsCompleted == showCompleted);
            }

            // Сортировка по дате и выполнение запроса
            var result = await tasks.OrderByDescending(t => t.CreatedDate).ToListAsync();
            
            // Сохранение параметров фильтрации в ViewData
            ViewData["CurrentFilter"] = searchString;
            ViewData["ShowCompleted"] = showCompleted;

            return View(result);
        }

        // GET: Tasks/Create - форма создания
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create - обработка формы
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Description,IsCompleted")] TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                taskItem.CreatedDate = DateTime.Now;
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: Tasks/Edit/5 - форма редактирования
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var taskItem = await _context.Tasks.FindAsync(id);
            if (taskItem == null) return NotFound();

            return View(taskItem);
        }

        // POST: Tasks/Edit/5 - обработка формы
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
            [Bind("Id,Title,Description,IsCompleted,CreatedDate")] TaskItem taskItem)
        {
            if (id != taskItem.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskItem);
        }

        // GET: Tasks/Delete/5 - подтверждение удаления
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var taskItem = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskItem == null) return NotFound();

            return View(taskItem);
        }

        // POST: Tasks/Delete/5 - удаление записи
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.Tasks.FindAsync(id);
            _context.Tasks.Remove(taskItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Проверка существования задачи
        private bool TaskItemExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if(task == null)
            {
                return NotFound();
            }

            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}