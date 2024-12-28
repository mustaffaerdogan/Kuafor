using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KuaforApp.Models
{
    /// <summary>
    /// Randevu bilgilerini tutan model sınıfı
    /// </summary>
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public int SalonID { get; set; }

        [Required]
        public int ServiceID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [Display(Name = "Randevu Saati")]
        public string TimeSlot { get; set; }

        [Required]
        [Display(Name = "Durum")]
        public AppointmentStatus Status { get; set; }

        [Required]
        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }

        [ForeignKey("SalonID")]
        public Salon Salon { get; set; }

        [ForeignKey("ServiceID")]
        public Service Service { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }
    }

    public enum AppointmentStatus
    {
        [Display(Name = "Beklemede")]
        Pending = 0,

        [Display(Name = "Onaylandı")]
        Approved = 1,

        [Display(Name = "Reddedildi")]
        Rejected = 2
    }
} 