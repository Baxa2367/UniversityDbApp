
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
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Position { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; } = null!;
    }
}

