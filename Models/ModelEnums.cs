namespace HRManagment.Models
{
    public enum GenderType
    {
        Male,
        Female
    }
    public enum RoleType
    {
        Admin,
        Manager,
        Employee
    }
    public enum LeaveRequestStatus
    {
        Pending,
        Approval,
        Rejected
    }
    public enum AuditLogAction
    {
        Add,
        Remove,
        Modify
    }
    public enum AttendanceStatus
    {
        Present,
        Absent,
        SickLeave,
        Vacation
    }
    public enum Position
    {
        Manager,
        Secratry,
        Leader,
        SEO,
        SeniorDeveloper,
        JuniorDeveloper,
        Employee,
    }
}
