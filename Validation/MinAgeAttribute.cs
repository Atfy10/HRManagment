using System.ComponentModel.DataAnnotations;

namespace HRManagment.Validation
{
    public class MinAgeAttribute : ValidationAttribute
    {
        int _minAge;
        public MinAgeAttribute(int minAg)
        {
            _minAge = minAg;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is int age)
            {
                if (_minAge < age)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult($"Age must be at least {_minAge} years.");
        }
    }
}
