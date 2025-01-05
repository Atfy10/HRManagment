namespace HRManagment.Models
{
    public enum Governorate
    {
        Cairo,
        Alexandria,
        Mansoura,
        Menoufia,
        Tanta,
        Asyut,
        Benha
    }

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
        General,
        Add,
        Update,
        Delete,
        Display,
        Get,
        Set
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
        SeniorDesigner,
        JuniorDesigner,
        SeniorDeveloper,
        JuniorDeveloper,
        Employee,
    }
}
