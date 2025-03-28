using AutoMapper;
using HRManagment.Models;
using HRManagment.ViewModels;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace HRManagment.Services
{
    public delegate void NewDbOperationEventHandler(object sender, AuditLog e);

    public class EmployeeServices(
        HRManagmentContext context,
        IMapper mapper,
        ILogger<EmployeeServices> logger,
        IEmployeeValidationService validationService,
        DropDownService dropDownService)
        : IEmployeeService
    {

        public event NewDbOperationEventHandler DbChanged;

        //public enum CurrentOperation
        //{
        //    General,
        //    Add,
        //    Update,
        //    Delete,
        //    Display
        //}

        readonly HRManagmentContext _context = context;
        readonly IMapper _mapper = mapper;
        readonly ILogger<EmployeeServices> _logger = logger;
        readonly DropDownService _dropDownService = dropDownService;
        readonly IEmployeeValidationService _validationService = validationService;
        async Task<OperationResult<Employee>> ExecuteOperationAsync(AuditLogAction currentOp, Func<AuditLogAction, Task<OperationResult<Employee>>> action)
        {
            try
            {
                OperationResult<Employee> result = await action(currentOp);
                result.Operation = currentOp;
                if (result.Success)
                {
                    _logger.LogInformation("Operation {Operation} has done succefully", currentOp);

                    if (currentOp is not AuditLogAction.Get)
                    {
                        //OperationResult<Employee> employee = new OperationResult<Employee>();
                        AuditLog newOperation = PopulateNewOperation(result, currentOp);
                        DbChanged?.Invoke(this, newOperation);
                    }
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

                return new OperationResult<Employee>
                {
                    Success = false,
                    Message = $"Database error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Operation {Operation} failed unexceptedly", currentOp);

                return new OperationResult<Employee>
                {
                    Success = false,
                    Message = $"Unexpected error: {ex.Message}, {ex.InnerException}"
                };

            }
        }

        public async Task<(List<Employee>, int)> GetEmployeesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null,
            string? sortBy = null,
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

            return (employees, pagesTotalCount);
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
                #region Validation Section

                /*********  Validation Section *********/
                if (!_validationService.IsEmailValid(employee.Email))
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

                #endregion

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return new OperationResult<Employee>
                {
                    Success = true,
                    Data = employee,
                    OldData = new Employee(),
                    Message = "Employee added succefully"
                };

            });

        }

        public async Task<OperationResult<Employee>> UpdateEmployeeAsync(Employee employee, Employee oldEmployee)
        {
            return await ExecuteOperationAsync(AuditLogAction.Update, async (currentOp) =>
            {
                #region Validation Section
                /*********  Validation Section *********/
                if (employee.SSN != oldEmployee.SSN)
                {
                    return new OperationResult<Employee>
                    {
                        Success = false,
                        Message = $"SSN: {employee.SSN} is not valid because it changed"
                    };
                }
                if (!_validationService.IsEmailValid(employee.Email, employee.Id))
                {
                    return new OperationResult<Employee>
                    {
                        Success = false,
                        Message = $"Email: {employee.Email} is already exist"
                    };
                }

                #endregion

                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return new OperationResult<Employee>
                {
                    Success = true,
                    OldData = oldEmployee,
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
                    OldData = employee,
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

        AuditLog SetAuditLogInfo(int userId, AuditLogAction currentOp,
            Employee newEmployee, Employee oldEmployee)
        {
            string newValue = newEmployee - oldEmployee;
            return new AuditLog
            {
                Action = currentOp,
                NewValue = newValue ?? string.Empty,
                OldValue = oldEmployee?.ToString() ?? "No data",
                TableName = "Employees",
                ChangedAt = DateTime.Now,
                ChangedBy = userId,
                PrimaryKeyValue = newEmployee?.Id.ToString() ?? oldEmployee?.Id.ToString(),
            };
        }

        private AuditLog PopulateNewOperation(OperationResult<Employee> result,
                                AuditLogAction currentOp) //where T : Employee
            => currentOp switch
            {
                AuditLogAction.Add =>
                    SetAuditLogInfo(1, currentOp, result.Data, null),
                AuditLogAction.Update =>
                    SetAuditLogInfo(1, currentOp, result.Data, result?.OldData),
                AuditLogAction.Delete =>
                    SetAuditLogInfo(1, currentOp, null, result.OldData),
                _ => new AuditLog()
                {
                    Action = currentOp
                }
            };

    }

};