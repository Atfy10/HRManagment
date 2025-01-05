using HRManagment.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HRManagment.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Length(14, 14)]
        public string SSN { get; set; }

        [Required]
        [StringLength(50)]
        public string FName { get; set; }

        [Required]
        [StringLength(50)]
        public string LName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; }

        public string Position { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Salary { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public GenderType Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Phone]
        public string EmergencyContact { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        //Navigation Property
        public virtual Department Department { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<LeaveRequest> RequestedLeaveRequests { get; set; }
        public virtual ICollection<LeaveRequest> ApprovededLeaveRequests { get; set; }
    }
}
