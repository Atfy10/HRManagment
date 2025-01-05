using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagment.Models
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime ApprovalDate { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

        [StringLength(200)]
        public string? Reason { get; set; }

        public string? Comments { get; set; }

        [Required]
        [ForeignKey("RequestingEmployee")]
        public int EmployeeId { get; set; }

        [ForeignKey("ApprovingEmployee")]
        public int? ApprovedById { get; set; }

        //Navigation Property
        public virtual Employee RequestingEmployee { get; set; }
        public virtual Employee ApprovingEmployee { get; set; }

    }
}
