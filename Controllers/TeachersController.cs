
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

        // Оқытушылар тізімі
        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Teachers
                .Include(t => t.Department)
                .AsNoTracking()
                .OrderBy(t => t.FullName)
                .ToListAsync();

            return View(teachers);
        }

        // Оқытушы қосу беті
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDepartments();
            return View();
        }

        // Оқытушыны сақтау
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (!string.IsNullOrWhiteSpace(teacher.Email))
            {
                teacher.Email = teacher.Email.Trim();

                bool emailExists = await _context.Teachers
                    .AnyAsync(t => t.Email == teacher.Email);

                if (emailExists)
                {
                    ModelState.AddModelError(
                        nameof(teacher.Email),
                        "Бұл электрондық пошта бұрын тіркелген."
                    );
                }
            }

            bool departmentExists = await _context.Departments
                .AnyAsync(d => d.Id == teacher.DepartmentId);

            if (!departmentExists)
            {
                ModelState.AddModelError(
                    nameof(teacher.DepartmentId),
                    "Кафедраны дұрыс таңдаңыз."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadDepartments(teacher.DepartmentId);
                return View(teacher);
            }

            try
            {
                _context.Teachers.Add(teacher);
                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Оқытушы дерекқорға сәтті қосылды.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                _context.Entry(teacher).State =
                    EntityState.Detached;

                ModelState.AddModelError(
                    string.Empty,
                    "Дерекқорға сақтау кезінде қате пайда болды. " +
                    "Енгізілген мәліметтерді тексеріңіз."
                );

                await LoadDepartments(teacher.DepartmentId);
                return View(teacher);
            }
        }

        // Өңдеу беті
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
                return NotFound();

            await LoadDepartments(teacher.DepartmentId);
            return View(teacher);
        }

        // Мәліметтерді жаңарту
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

            if (!string.IsNullOrWhiteSpace(teacher.Email))
            {
                teacher.Email = teacher.Email.Trim();

                bool emailExists = await _context.Teachers
                    .AnyAsync(t =>
                        t.Email == teacher.Email && t.Id != id);

                if (emailExists)
                {
                    ModelState.AddModelError(
                        nameof(teacher.Email),
                        "Бұл электрондық пошта басқа оқытушыға тіркелген."
                    );
                }
            }

            bool departmentExists = await _context.Departments
                .AnyAsync(d => d.Id == teacher.DepartmentId);

            if (!departmentExists)
            {
                ModelState.AddModelError(
                    nameof(teacher.DepartmentId),
                    "Кафедраны дұрыс таңдаңыз."
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

            try
            {
                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Оқытушы мәліметтері сәтті жаңартылды.";

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Мәліметтерді жаңарту кезінде қате пайда болды."
                );

                await LoadDepartments(teacher.DepartmentId);
                return View(teacher);
            }
        }

        // Жоюды растау беті
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return NotFound();

            return View(teacher);
        }

        // Оқытушыны жою
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
                return NotFound();

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Оқытушы жүйеден сәтті жойылды.";

            return RedirectToAction(nameof(Index));
        }

        // Кафедралар тізімін жүктеу
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

            ViewBag.HasDepartments = departments.Any();
        }
    }
}