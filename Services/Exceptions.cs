namespace HRManagment.Services
{
    public class AddNewEmployeeException(string? message, Exception? innerException) : Exception(message, innerException)
    {
    }

    public class UpdateEmployeeException(string? message, Exception? innerException) : Exception(message, innerException)
    {
    }

    public class DeleteEmployeeException(string? message, Exception? innerException) : Exception(message, innerException)
    {
    }
}
