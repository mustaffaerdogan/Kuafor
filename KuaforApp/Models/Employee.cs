using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuaforApp.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salon seçimi zorunludur.")]
        [Display(Name = "Salon")]
        public int SalonID { get; set; }

        [ForeignKey("SalonID")]
        public virtual Salon? Salon { get; set; }

        [Required(ErrorMessage = "Çalışma günleri zorunludur.")]
        [Display(Name = "Çalışma Günleri")]
        [StringLength(255, ErrorMessage = "Çalışma günleri 255 karakterden uzun olamaz.")]
        public string WorkingDays { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açılış saati zorunludur.")]
        [Display(Name = "Açılış Saati")]
        [DataType(DataType.Time)]
        public TimeSpan OpeningHours { get; set; }

        [Required(ErrorMessage = "Kapanış saati zorunludur.")]
        [Display(Name = "Kapanış Saati")]
        [DataType(DataType.Time)]
        public TimeSpan ClosingHours { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for employee services
        public virtual ICollection<EmployeeService>? EmployeeServices { get; set; }

        // Helper methods for working days
        public List<string> GetWorkingDaysList()
        {
            return WorkingDays?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
        }

        public void SetWorkingDaysList(List<string> days)
        {
            WorkingDays = days != null ? string.Join(",", days) : string.Empty;
        }
    }
} 