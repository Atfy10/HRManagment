using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

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

        [AllowNull]
        public string Notes { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Late Minutes cannot be negative")]
        [DisplayName("Late Minutes")]
        public int LateMinutes { get; set; } = 0;

        [DisplayName("Is Late")]
        public bool? IsLate { get; set; }


        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        //Navigation Property
        public virtual Employee Employee { get; set; }


    }
}
