using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagment.Models
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly CheckInTime { get; set; }

        [DataType(DataType.Time)]
        public TimeOnly CheckOutTime { get; set; }

        public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        //Navigation Property
        public virtual Employee Employee { get; set; }


    }
}
