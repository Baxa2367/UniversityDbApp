
using System.ComponentModel.DataAnnotations;

namespace UniversityDbApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Position { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }

        // Навигациялық қасиет
        public Department Department { get; set; } = null!;
    }
}
