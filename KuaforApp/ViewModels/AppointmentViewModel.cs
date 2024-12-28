using System.ComponentModel.DataAnnotations;
using KuaforApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KuaforApp.ViewModels
{
    /// <summary>
    /// Randevu oluşturma ve düzenleme için view model
    /// </summary>
    public class AppointmentViewModel
    {
        [Required(ErrorMessage = "Salon seçimi zorunludur.")]
        [Display(Name = "Salon")]
        public int SalonID { get; set; }

        [Required(ErrorMessage = "Hizmet seçimi zorunludur.")]
        [Display(Name = "Hizmet")]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "Çalışan seçimi zorunludur.")]
        [Display(Name = "Çalışan")]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Tarih seçimi zorunludur.")]
        [Display(Name = "Tarih")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "Saat seçimi zorunludur.")]
        [Display(Name = "Saat")]
        public string TimeSlot { get; set; }

        // Dropdown lists for the form
        public SelectList Salons { get; set; }
        public SelectList Services { get; set; }
        public SelectList Employees { get; set; }
        public List<string> AvailableTimeSlots { get; set; }

        public AppointmentViewModel()
        {
            AppointmentDate = DateTime.Today;
            AvailableTimeSlots = new List<string>();
        }
    }

    /// <summary>
    /// Randevu listeleme için view model
    /// </summary>
    public class AppointmentListViewModel
    {
        public int AppointmentID { get; set; }
        public string SalonName { get; set; }
        public string ServiceName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Helper property to display status in Turkish
        public string StatusText => Status switch
        {
            AppointmentStatus.Pending => "Beklemede",
            AppointmentStatus.Approved => "Onaylandı",
            AppointmentStatus.Rejected => "Reddedildi",
            _ => "Bilinmiyor"
        };
    }
} 