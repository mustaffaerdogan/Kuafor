using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KuaforApp.Models;
using KuaforApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KuaforApp.Controllers
{
    /// <summary>
    /// Admin paneli için controller sınıfı.
    /// Sadece Admin rolüne sahip kullanıcılar erişebilir.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Admin dashboard sayfasını gösterir.
        /// </summary>
        public async Task<IActionResult> Dashboard()
        {
            var viewModel = new AdminDashboardViewModel
            {
                TotalSalons = await _context.Salons.CountAsync(),
                TotalServices = await _context.Services.CountAsync(),
                RecentSalons = await _context.Salons
                    .OrderByDescending(s => s.CreatedAt)
                    .Take(5)
                    .ToListAsync(),
                RecentServices = await _context.Services
                    .Include(s => s.Salon)
                    .OrderByDescending(s => s.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Yetkilendirme hatası durumunda gösterilecek sayfa.
        /// </summary>
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
} 