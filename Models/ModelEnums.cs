using System.ComponentModel;
using System.Reflection;

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
        SEO,
        Senior,
        Junior,
        Fresh,
        Intern,
        Employee,
        [Description("Team Lead")]
        TeamLead,
        [Description("Software Engineer")]
        SoftwareEngineer,
        [Description("Marketing Specialist")]
        MarketingSpecialist,
        [Description("Data Analyst")]
        DataAnalyst,
        [Description("System Administrator")]
        SystemAdministrator,
        [Description("Graphic Designer")]
        GraphicDesigner
    }

    public static class EnumsExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType()?.GetField(value.ToString());
            if (field != null)
            {
                var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                return attribute == null ? value.ToString() : attribute.Description;
            }
            return value.ToString();
        }

        public static string GetDescription<TEnum>(this string enumValue) where TEnum : struct, Enum
        {
            if (Enum.TryParse(enumValue, out TEnum value))
            {
                FieldInfo? field = typeof(TEnum).GetField(value.ToString());
                if (field != null)
                {
                    var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                    return attribute?.Description ?? value.ToString();
                }
            }
            return enumValue;
        }
    }
    public enum LeaveType
    {
        Sick,
        Casual,
        Annual,
        Maternity,
        Paternity,
        Unpaid
    }
    public enum DepartmentType
    {
        HR,
        IT,
        Marketing,
        Sales,
        Finance,
        Operations,
        Production,
        Quality,
        Maintenance,
        Administration,
        [Description("Customer Service")]
        CustomerService,
        [Description("Public Relations")]
        PublicRelations,
        [Description("Human Resources")]
        HumanResources,
        [Description("Information Technology")]
        InformationTechnology,
        [Description("Quality Assurance")]
        QualityAssurance,
        [Description("Research and Development")]
        ResearchAndDevelopment,
        [Description("Sales and Marketing")]
        SalesAndMarketing,
        [Description("Supply Chain")]
        SupplyChain,
        [Description("Technical Support")]
        TechnicalSupport,
        [Description("Training and Development")]
        TrainingAndDevelopment,
        [Description("Warehouse and Logistics")]
        WarehouseAndLogistics
    }
}
