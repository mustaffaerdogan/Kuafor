using KuaforApp.Models;

namespace KuaforApp.ViewModels
{
    /// <summary>
    /// Admin dashboard sayfası için view model.
    /// </summary>
    public class AdminDashboardViewModel
    {
        public int TotalSalons { get; set; }
        public int TotalServices { get; set; }
        public int PendingAppointments { get; set; }
        public List<Salon> RecentSalons { get; set; } = new();
        public List<Service> RecentServices { get; set; } = new();
    }
} 