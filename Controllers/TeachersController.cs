
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityDbApp.Data;
using UniversityDbApp.Models;

namespace UniversityDbApp.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        // Барлық оқытушылар тізімі
        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Department)
                .AsNoTracking()
                .OrderBy(t => t.FullName)
                .ToListAsync();

            return View(teachers);
        }

        // Жаңа оқытушы қосу беті
        public async Task<IActionResult> Create()
        {
            await LoadDepartments();
            return View();
        }

        // Жаңа оқытушыны сақтау
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (await _context.Teachers.AnyAsync(
                t => t.Email == teacher.Email))
            {
                ModelState.AddModelError(
                    "Email",
                    "Бұл электрондық пошта бұрын тіркелген."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadDepartments(teacher.DepartmentId);
                return View(teacher);
            }

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Оқытушы сәтті қосылды.";

            return RedirectToAction(nameof(Index));
        }

        // Оқытушы мәліметін өңдеу беті
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
                return NotFound();

            await LoadDepartments(teacher.DepartmentId);

            return View(teacher);
        }

        // Оқытушы мәліметін жаңарту
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, Teacher teacher)
        {
            if (id != teacher.Id)
                return BadRequest();

            var existingTeacher = await _context.Teachers
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTeacher == null)
                return NotFound();

            if (await _context.Teachers.AnyAsync(
                t => t.Email == teacher.Email && t.Id != id))
            {
                ModelState.AddModelError(
                    "Email",
                    "Бұл электрондық пошта басқа оқытушыға тіркелген."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadDepartments(teacher.DepartmentId);
                return View(teacher);
            }

            existingTeacher.FullName = teacher.FullName;
            existingTeacher.Email = teacher.Email;
            existingTeacher.Position = teacher.Position;
            existingTeacher.DepartmentId = teacher.DepartmentId;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Оқытушы мәліметтері жаңартылды.";

            return RedirectToAction(nameof(Index));
        }

        // Оқытушыны жоюды растау беті
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        // Оқытушыны дерекқордан жою
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers
                .FindAsync(id);

            if (teacher == null)
                return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Оқытушы жүйеден жойылды.";

            return RedirectToAction(nameof(Index));
        }

        // Кафедраларды таңдау тізіміне жүктеу
        private async Task LoadDepartments(int? selectedId = null)
        {
            var departments = await _context.Departments
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .ToListAsync();

            ViewBag.DepartmentId = new SelectList(
                departments,
                "Id",
                "Name",
                selectedId
            );
        }
    }
}