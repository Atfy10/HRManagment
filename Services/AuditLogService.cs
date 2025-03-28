using HRManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace HRManagment.Services
{
    public class AuditLogService(HRManagmentContext context, 
        IEmployeeService employeeService, 
        ILogger<AuditLogService> logger) : IAuditLogService
    {
        readonly HRManagmentContext _context = context;
        readonly IEmployeeService _employeeService = employeeService;
        readonly ILogger<AuditLogService> _logger = logger;


        public async Task SubscribeToOperationEvent()
        {

            _employeeService.DbChanged += async (sender, a) =>
            {
                _logger.LogInformation("Sender is: {Sender}, with action: {Action}\n EmployeeNewData: {Employee}" +
                    "\n TableName: {TableName}", sender.ToString(), a.Action, a.NewValue, a.TableName);
                try
                {
                    await _context.AuditLogs.AddAsync(a);
                    _context.SaveChanges();
                    _logger.LogInformation("New {Action} operation saved successfully", a.Action);
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogWarning("Register {Action} operation to database failed due {Exception}", a.Action, ex.Message);

                }
                //>	HRManagment.dll!HRManagment.Services.AuditLogService.SubscribeToOperationEvent.AnonymousMethod__4_0(object sender, HRManagment.Models.AuditLog a) Line 33	C#

                catch (Exception ex)
                {
                    _logger.LogWarning("Unexpected error during register {Action} operation: {Exception}", a.Action, ex.Message);

                }
            };

        }
    }
}
