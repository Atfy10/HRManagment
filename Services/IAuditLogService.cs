using HRManagment.Models;

namespace HRManagment.Services
{
    public interface IAuditLogService
    {
        Task SubscribeToOperationEvent();
    }
}
