namespace HRManagment.Services
{
    public interface IEmployeeValidationService
    {
        bool IsEmailValid(string email, int? exceptEmployeeId = null);
        bool IsValidSSN(string ssn, DateTime birthDate, string address, int? exceptEmployeeId = null);
        //bool IsValidAddress(string address);
    }
}
