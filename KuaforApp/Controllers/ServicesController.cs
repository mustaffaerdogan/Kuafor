using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KuaforApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace KuaforApp.Controllers
{
    /// <summary>
    /// Kuaför/Berber hizmetlerinin yönetimini sağlayan controller sınıfı.
    /// Bu controller hizmet ekleme, düzenleme, silme ve listeleme işlemlerini yönetir.
    /// Her hizmet bir salona bağlıdır.
    /// </summary>
    [Authorize]
    public class ServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// ServicesController constructor'ı.
        /// </summary>
        /// <param name="context">Veritabanı context'i</param>
        public ServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tüm hizmetleri listeler.
        /// Hizmetler önce salon adına, sonra hizmet adına göre sıralanır.
        /// Her hizmet için bağlı olduğu salon bilgisi de yüklenir.
        /// </summary>
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Index()
        {
            var services = await _context.Services
                .Include(s => s.Salon)
                .OrderBy(s => s.Salon.SalonName)
                .ThenBy(s => s.ServiceName)
                .ToListAsync();
            return View(services);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip hizmetin detaylarını gösterir.
        /// </summary>
        /// <param name="id">Hizmet ID</param>
        /// <returns>Hizmet bulunamazsa NotFound, bulunursa Details view'ı döner</returns>
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Salon)
                .FirstOrDefaultAsync(m => m.ServiceID == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        /// <summary>
        /// Yeni hizmet ekleme formunu gösterir.
        /// Salon seçimi için gerekli verileri ViewBag ile view'a aktarır.
        /// </summary>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.SalonID = new SelectList(_context.Salons.OrderBy(s => s.SalonName), "SalonID", "SalonName");
            return View();
        }

        /// <summary>
        /// Yeni hizmet kaydı oluşturur.
        /// </summary>
        /// <param name="service">Oluşturulacak hizmet bilgileri</param>
        /// <returns>Başarılı ise Index'e yönlendirir, hata varsa formu tekrar gösterir</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ServiceName,Duration,Price,Description,SalonID")] Service service)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.CreatedAt = DateTime.UtcNow;
                    _context.Add(service);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Hizmet başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Hizmet kaydedilirken bir hata oluştu: " + ex.Message);
                }
            }

            ViewBag.SalonID = new SelectList(_context.Salons.OrderBy(s => s.SalonName), "SalonID", "SalonName", service.SalonID);
            return View(service);
        }

        /// <summary>
        /// Hizmet düzenleme formunu gösterir.
        /// </summary>
        /// <param name="id">Düzenlenecek hizmet ID</param>
        /// <returns>Hizmet bulunamazsa NotFound, bulunursa Edit view'ı döner</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            ViewBag.SalonID = new SelectList(_context.Salons.OrderBy(s => s.SalonName), "SalonID", "SalonName", service.SalonID);
            return View(service);
        }

        /// <summary>
        /// Hizmet bilgilerini günceller.
        /// </summary>
        /// <param name="id">Güncellenecek hizmet ID</param>
        /// <param name="service">Güncellenmiş hizmet bilgileri</param>
        /// <returns>Başarılı ise Index'e yönlendirir, hata varsa formu tekrar gösterir</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceID,ServiceName,Duration,Price,Description,SalonID")] Service service)
        {
            if (id != service.ServiceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingService = await _context.Services.FindAsync(id);
                    if (existingService == null)
                    {
                        return NotFound();
                    }

                    // Update only the modifiable properties
                    existingService.ServiceName = service.ServiceName;
                    existingService.Duration = service.Duration;
                    existingService.Price = service.Price;
                    existingService.Description = service.Description;
                    existingService.SalonID = service.SalonID;

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Hizmet başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.ServiceID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Hizmet güncellenirken bir hata oluştu: " + ex.Message);
                }
            }

            ViewBag.SalonID = new SelectList(_context.Salons.OrderBy(s => s.SalonName), "SalonID", "SalonName", service.SalonID);
            return View(service);
        }

        /// <summary>
        /// Hizmet silme onay formunu gösterir.
        /// </summary>
        /// <param name="id">Silinecek hizmet ID</param>
        /// <returns>Hizmet bulunamazsa NotFound, bulunursa Delete view'ı döner</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services
                .Include(s => s.Salon)
                .FirstOrDefaultAsync(m => m.ServiceID == id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        /// <summary>
        /// Hizmeti siler.
        /// </summary>
        /// <param name="id">Silinecek hizmet ID</param>
        /// <returns>Başarılı ise Index'e yönlendirir, hata varsa Index'e hata mesajıyla yönlendirir</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            try
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Hizmet başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hizmet silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip hizmetin var olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="id">Kontrol edilecek hizmet ID</param>
        /// <returns>Hizmet varsa true, yoksa false döner</returns>
        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.ServiceID == id);
        }
    }
} 