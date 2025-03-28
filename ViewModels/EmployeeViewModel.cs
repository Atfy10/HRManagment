using HRManagment.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using HRManagment.Validation;

namespace HRManagment.ViewModels
{
    [Bind("Id,FName,LName,SSN,EmergencyContact,Email,Phone,HireDate,DateOfBirth,Position,Salary,Address,Gender,DepartmentId")]
    public class EmployeeViewModel
    {
        [AllowNull]
        public int Id { get; set; }

        [Required]
        [Length(14, 14)]
        public string SSN { get; set; }

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

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Phone]
        public string EmergencyContact { get; set; }

        [DataType(DataType.Date)]
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
        public IEnumerable<SelectListItem> Governorates { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Positions { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Genders { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> Departments { get; set; }


        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var emailPattern = @"^[a-zA-Z0-9._%+]+@[a-zA-Z0-9.-]\.[a-zA-Z]{2,}$";

        //    if (FName == "" || FName == null || FName.Length > 49)
        //        yield return new ValidationResult("FName must be between 3 and 50 characters",
        //            [nameof(FName)]);
        //    if (LName == "" || LName == null || LName.Length > 49)
        //        yield return new ValidationResult("LName must be between 3 and 50 characters",
        //            [nameof(LName)]);
        //    if (!Regex.IsMatch(Email, emailPattern))
        //        yield return new ValidationResult("Email syntax isn't correct",
        //            [nameof(Email)]);
        //    if (Salary < 0)
        //        yield return new ValidationResult("Salary must be larger than 0",
        //            [nameof(Salary)]);
        //    if (!Enum.IsDefined<Position>(Position))
        //        yield return new ValidationResult("Position you entered not exist",
        //            [nameof(Position)]);
        //    if (!Enum.IsDefined<GenderType>(Gender))
        //        yield return new ValidationResult("Gender you entered not exist",
        //            [nameof(Gender)]);
        //}
    }
}
