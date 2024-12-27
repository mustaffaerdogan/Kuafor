using System.ComponentModel.DataAnnotations;
using KuaforApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KuaforApp.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        [StringLength(100, ErrorMessage = "Ad Soyad en fazla 100 karakter olabilir.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salon seçimi zorunludur.")]
        [Display(Name = "Salon")]
        public int SalonID { get; set; }

        // Read-only properties that will be populated from the selected salon
        [Display(Name = "Çalışma Günleri")]
        public string WorkingDays { get; set; } = string.Empty;

        [Display(Name = "Açılış Saati")]
        public TimeSpan OpeningHours { get; set; }

        [Display(Name = "Kapanış Saati")]
        public TimeSpan ClosingHours { get; set; }

        // Lists for dropdowns and display
        public SelectList? SalonList { get; set; }
        public List<string>? AvailableWorkingDays { get; set; }
        public List<Service>? SalonServices { get; set; }

        // Helper property to display working days in a readable format
        public string FormattedWorkingDays => string.Join(", ", WorkingDays.Split(',').Select(d => d.Trim()));
    }
} 