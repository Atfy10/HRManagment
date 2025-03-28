using System.ComponentModel.DataAnnotations;

namespace HRManagment.Validation
{
    public class SSNAttribute : ValidationAttribute
    {
        public SSNAttribute() { }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = "SSN length must be 14";
            var validationResult = new ValidationResult(ErrorMessage);
            if (value is not null && value is string ssn)
            {
                if (ssn.Length != 14)
                {
                    return validationResult;
                }
                if (ssn[0] > 3 || ssn[0] < 2)
                {
                    validationResult.ErrorMessage = "First digit must be 2 or 3";
                    return validationResult;
                }

                return ValidationResult.Success;
            }

            return validationResult;
        }
    }
}
