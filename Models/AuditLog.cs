using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRManagment.Models
{
    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public AuditLogAction Action { get; set; }

        [Required]
        public string TableName { get; set; }

        [Required]
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string OldValue { get; set; }

        [Required]
        public string NewValue { get; set; }

        public string PrimaryKeyValue { get; set; }

        [Required]
        [ForeignKey("ChangedByUser")]
        public int ChangedBy { get; set; }

        //Navigation Property
        public virtual User ChangedByUser { get; set; }
    }
}
