using HRManagment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace HRManagment.Services
{
    public class EmployeeValidationService(HRManagmentContext context) : IEmployeeValidationService
    {
        HRManagmentContext _context = context;
        public bool IsEmailValid(string email, int? exceptEmployeeId = null)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                //  if found --> return !true (false)
                return !_context.Employees
                    .Where(e => e.Id != exceptEmployeeId)
                    .Any(e => e.Email == email);
            }
            return false;
        }

        public bool IsValidSSN(string ssn, DateTime birthDate, string address, int? exceptEmployeeId = null)
        {
            //  3 040303 17 01939
            //  2 991229 17 01939
            if (!_context.Employees
                .Where(e => e.Id != exceptEmployeeId)
                .Any(e => e.SSN == ssn))
            {
                var year = birthDate.Year;
                var actualBirthDate = birthDate.ToString("yyMMdd");
                var firstDigit = Convert.ToInt32(ssn[0].ToString());
                var ssnBirthDate = ssn.Substring(1, 6);
                var ssnGovernrateCode = Convert.ToInt32(ssn.Substring(7, 2));

                if (GetSupposedFirstDigit(year) != firstDigit)
                    return false;

                if (!actualBirthDate.Equals(ssnBirthDate))
                    return false;

                if (GetSupposedAddressCode(address) != ssnGovernrateCode)
                    return false;

                return true;
            }

            return false;
        }

        /*******************  SSN validation helper method  *******************/
        int GetSupposedFirstDigit(int year) => year > 1999 ? 3 : 2;
        int GetSupposedAddressCode(string address)
        {
            if (Enum.TryParse(address, out Governorate governorate))
                return governorate switch
                {
                    Governorate.Cairo => 01,
                    Governorate.Alexandria => 02,
                    Governorate.Menoufia => 17,
                    Governorate.Tanta => 16,
                    Governorate.Benha => 14,
                    Governorate.Asyut => 25,
                    Governorate.Mansoura => 12,
                    _ => 0
                };
            return 0;
        }
    }
}
