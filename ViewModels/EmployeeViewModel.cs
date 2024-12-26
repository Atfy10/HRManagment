using HRManagment.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace HRManagment.ViewModels
{
    public class EmployeeViewModel
    {
        [AllowNull]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First Name")]
        public string FName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last Name")]
        public string LName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public DateTime? HireDate { get; set; }

        public string Position { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
        public decimal? Salary { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [Display(Name = "Gender")]
        public GenderType Gender { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }


        [BindNever]
        public IEnumerable<SelectListItem> Positions { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Genders { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Departments { get; set; }

    }
}
