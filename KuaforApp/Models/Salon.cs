using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KuaforApp.Models
{
    public class Salon
    {
        [Key]
        public int SalonID { get; set; }

        [Required(ErrorMessage = "Salon adı zorunludur.")]
        [Display(Name = "Salon Adı")]
        [StringLength(100, ErrorMessage = "Salon adı en fazla 100 karakter olabilir.")]
        public string SalonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konum bilgisi zorunludur.")]
        [Display(Name = "Konum")]
        [StringLength(255, ErrorMessage = "Konum bilgisi en fazla 255 karakter olabilir.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Açılış saati zorunludur.")]
        [Display(Name = "Açılış Saati")]
        [DataType(DataType.Time)]
        public TimeSpan OpeningHours { get; set; }

        [Required(ErrorMessage = "Kapanış saati zorunludur.")]
        [Display(Name = "Kapanış Saati")]
        [DataType(DataType.Time)]
        public TimeSpan ClosingHours { get; set; }

        [Required(ErrorMessage = "Çalışma günleri zorunludur.")]
        [Display(Name = "Çalışma Günleri")]
        [StringLength(255, ErrorMessage = "Çalışma günleri 255 karakterden uzun olamaz.")]
        public string WorkingDays { get; set; } = string.Empty;

        [Display(Name = "Salon Resmi")]
        [StringLength(1000)]
        public string? ImageUrl { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for services
        public virtual ICollection<Service>? Services { get; set; }

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