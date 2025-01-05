using AutoMapper;
using HRManagment.Models;
using HRManagment.ViewModels;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace HRManagment.Services
{
    public class EmployeeServices(
        HRManagmentContext context,
        IMapper mapper,
        ILogger<EmployeeServices> logger,
        IEmployeeValidationService validationService,
        DropDownService dropDownService)
        : IEmployeeService
    {
        //public enum CurrentOperation
        //{
        //    General,
        //    Add,
        //    Update,
        //    Delete,
        //    Display
        //}

        readonly HRManagmentContext _context = context;
        //readonly IMapper _mapper = mapper;
        readonly ILogger<EmployeeServices> _logger = logger;
        readonly DropDownService _dropDownService = dropDownService;
        readonly IEmployeeValidationService _validationService = validationService;
        async Task<OperationResult<T>> ExecuteOperationAsync<T>(AuditLogAction currentOp, Func<AuditLogAction, Task<OperationResult<T>>> action)
        {
            try
            {
                var result = await action(currentOp);
                result.Operation = currentOp;
                if (result.Success)
                {
                    _logger.LogInformation("Operation {Operation} has done succefully", currentOp);
                }
                else
                {
                    _logger.LogWarning("Operation: {Operation} failed: {Message}", currentOp, result.Message);
                }
                return result;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database {Operation} failed", currentOp);
                //throw new EmployeeOperationException($"Failed to {currentOp} employee", ex);
                return new OperationResult<T>
                {
                    Success = false,
                    Message = $"Database error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Operation {Operation} failed unexceptedly", currentOp);
                //throw new EmployeeOperationException($"Unexcepected error during {currentOp} employee", ex);
                return new OperationResult<T>
                {
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}, {ex.InnerException}"
                };

            }
        }

        public async Task<List<Employee>> GetEmployeesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string searchTerm = null,
            string sortBy = null,
            bool ascending = true)
        {

            var query = _context.Employees
                .Include(d => d.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(
                   e => e.FName == searchTerm ||
                   e.LName == searchTerm ||
                   e.Email == searchTerm);
            }

            var pagesTotalCount = await query.CountAsync();

            if (!string.IsNullOrEmpty(sortBy))
            {
                query = ascending ?
                    query.OrderBy(e => EF.Property<object>(e, sortBy)) :
                    query.OrderByDescending(e => EF.Property<object>(e, sortBy));
            }

            var employees = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return employees;
        }


        /*******************    Four main operations   *******************/
        public async Task<OperationResult<Employee>> GetEmployeeByIdAsync(int id)
        {
            return await ExecuteOperationAsync(AuditLogAction.Get, async (currentOp) =>
            {
                /*********  Validation Section *********/
                if (!IsValidId(id))
                {
                    return new OperationResult<Employee>
                    {
                        Success = false,
                        Message = $"Employee with {id} not found"
                    };
                }


                var employee = await _context.Employees
                    .Include(d => d.Department)
                    .SingleAsync(x => x.Id == id);

                return new OperationResult<Employee>
                {
                    Success = true,
                    Data = employee,
                    Message = "Employee found"
                };
            });
        }

        public async Task<OperationResult<Employee>> AddEmployeeAsync(Employee employee)
        {
            return await ExecuteOperationAsync(AuditLogAction.Add, async (currentOp) =>
            {

                /*********  Validation Section *********/

                if (!await _validationService.IsEmailUniqueAsync(employee.Email))
                {
                    return new OperationResult<Employee>
                    {
                        Success = false,
                        Message = $"Email: {employee.Email} is already exist"
                    };
                }
                if (!_validationService.IsValidSSN(employee.SSN,
                    employee.DateOfBirth ?? DateTime.Today, employee.Address ?? "Cairo"))
                {
                    return new OperationResult<Employee>
                    {
                        Success = false,
                        Message = $"SSN: {employee.SSN} is not valid or is exist"
                    };
                }
                //
                //
                //

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return new OperationResult<Employee>
                {
                    Success = true,
                    Data = employee,
                    Message = "Employee added succefully"
                };

            });

        }

        public async Task<OperationResult<Employee>> UpdateEmployeeAsync(Employee employee)
        {
            return await ExecuteOperationAsync(AuditLogAction.Update, async (currentOp) =>
            {

                /*********  Validation Section *********/

                if (!_validationService.IsValidSSN(employee.SSN,
                    employee.DateOfBirth ?? DateTime.Today, employee.Address ?? "Cairo",
                    employee.Id))
                {
                    return new OperationResult<Employee>
                    {
                        Success = false,
                        Message = $"SSN: {employee.SSN} is not valid or is exist"
                    };
                }
                if (!await _validationService.IsEmailUniqueAsync(employee.Email, employee.Id))
                {
                    return new OperationResult<Employee>
                    {
                        Success = false,
                        Message = $"Email: {employee.Email} is already exist"
                    };
                }
                //
                //
                //

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return new OperationResult<Employee>
                {
                    Success = true,
                    Data = employee,
                    Message = "Employee data was updated successfully"
                };
            });
        }

        public async Task<OperationResult<Employee>> DeleteEmployeeAsync(int id)
        {
            return await ExecuteOperationAsync(AuditLogAction.Delete, async (currentOp) =>
            {

                /*********  Validation Section *********/

                if (!IsValidId(id))
                {
                    return new OperationResult<Employee>
                    {
                        Success = false,
                        Message = "Employee not found"
                    };
                }
                //
                //
                //

                var employee = await _context.Employees.SingleAsync(e => e.Id == id);
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return new OperationResult<Employee>
                {
                    Success = true,
                    Data = employee,
                    Message = "Employee data was deleted successfully"
                };

            });
        }

        /*******************    Helper Methods   *******************/
        public bool IsValidId(int id)
        {
            if (id > 0 && _context.Employees.Any(e => e.Id == id))
                return true;

            return false;
        }

        public void PopulateDropDownLists(EmployeeViewModel viewModel)
        {
            viewModel.Governorates = _dropDownService.GetGovernorates();
            viewModel.Departments = _dropDownService.GetDepartments();
            viewModel.Positions = _dropDownService.GetPositions();
            viewModel.Genders = _dropDownService.GetGenders();
        }

    }



}

;