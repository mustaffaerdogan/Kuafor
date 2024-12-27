using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuaforApp.Models
{
    public class EmployeeService
    {
        [Key]
        public int EmployeeServiceID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public int ServiceID { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("ServiceID")]
        public virtual Service? Service { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
} 