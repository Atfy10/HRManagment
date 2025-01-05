using HRManagment.Models;
using HRManagment.ViewModels;
using static HRManagment.Services.EmployeeServices;

namespace HRManagment.Services
{
    public interface IEmployeeService
    {
        //public string CurrentOp { get; }
        Task<OperationResult<Employee>> GetEmployeeByIdAsync(int id);
        Task<List<Employee>> GetEmployeesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string searchTerm = null,
            string sortBy = null,
            bool ascending = true);
        Task<OperationResult<Employee>> AddEmployeeAsync(Employee employee);
        Task<OperationResult<Employee>> UpdateEmployeeAsync(Employee employee);
        Task<OperationResult<Employee>> DeleteEmployeeAsync(int id);
        void PopulateDropDownLists(EmployeeViewModel viewModel);
        //Task UpdateDbContextAsync(AuditLogAction currentOp);
        bool IsValidId(int id);
    }
}
