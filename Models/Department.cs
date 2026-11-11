
using System.ComponentModel.DataAnnotations;

namespace UniversityDbApp.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        // Бір кафедрада бірнеше оқытушы болуы мүмкін
        public ICollection<Teacher> Teachers { get; set; }
            = new List<Teacher>();
    }
}
