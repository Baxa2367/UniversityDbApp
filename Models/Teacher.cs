
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UniversityDbApp.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Оқытушының аты-жөнін енгізіңіз.")]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Электрондық поштаны енгізіңіз.")]
        [EmailAddress(ErrorMessage = "Электрондық пошта форматы дұрыс емес.")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Position { get; set; }

        [Required(ErrorMessage = "Кафедраны таңдаңыз.")]
        public int DepartmentId { get; set; }

        [ValidateNever]
        public Department Department { get; set; } = null!;
    }
}