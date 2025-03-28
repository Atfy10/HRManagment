using HRManagment.Models;
using HRManagment.ViewModels;
using static HRManagment.Services.EmployeeServices;

namespace HRManagment.Services
{
    public interface IEmployeeService
    {
        public event NewDbOperationEventHandler DbChanged;
        Task<OperationResult<Employee>> GetEmployeeByIdAsync(int id);
        Task<(List<Employee>, int)> GetEmployeesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string searchTerm = null,
            string sortBy = null,
            bool ascending = true);
        Task<OperationResult<Employee>> AddEmployeeAsync(Employee employee);
        Task<OperationResult<Employee>> UpdateEmployeeAsync(Employee employee, Employee oldEmployee);
        Task<OperationResult<Employee>> DeleteEmployeeAsync(int id);
        void PopulateDropDownLists(EmployeeViewModel viewModel);
        //Task UpdateDbContextAsync(AuditLogAction currentOp);
        bool IsValidId(int id);
    }
}
