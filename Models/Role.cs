using System.ComponentModel.DataAnnotations;

namespace HRManagment.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public RoleType Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        //Navigation Property
        public virtual ICollection<User> Users { get; set; }
    }
}
