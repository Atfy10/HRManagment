using HRManagment.Models;

namespace HRManagment.Services
{
    public class OperationResult<T>
    {
        public AuditLogAction Operation { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
