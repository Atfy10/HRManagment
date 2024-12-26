using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagment.Models
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

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

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        //Navigation Property
        public virtual Employee Employee { get; set; }

    }
}
