using HRManagment.Models;
using System.ComponentModel.DataAnnotations;

namespace HRManagment.ValidationAttributes
{
    public class UniqueAttribute(HRManagmentContext context) : ValidationAttribute
    {
        readonly HRManagmentContext _context = context;
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string { Length: > 0 } email)
            {
                var invalid = _context.Employees.Any(e => e.Email == email);
                return new ValidationResult("Email is already exist.");
            }
            return ValidationResult.Success;
        }
    }
}
