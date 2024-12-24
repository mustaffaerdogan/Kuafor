using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KuaforApp.Models;

namespace KuaforApp.Controllers
{
    /// <summary>
    /// Kuaför/Berber salonlarının yönetimini sağlayan controller sınıfı.
    /// Bu controller salon ekleme, düzenleme, silme ve listeleme işlemlerini yönetir.
    /// </summary>
    public class SalonsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string[] _workingDays = new[] { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar" };

        /// <summary>
        /// SalonsController constructor'ı.
        /// </summary>
        /// <param name="context">Veritabanı context'i</param>
        public SalonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tüm salonları listeler.
        /// Salonlar oluşturulma tarihine göre azalan sırada listelenir.
        /// Her salon için ilişkili hizmetler de yüklenir.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var salons = await _context.Salons
                .Include(s => s.Services)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
            return View(salons);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip salonun detaylarını gösterir.
        /// </summary>
        /// <param name="id">Salon ID</param>
        /// <returns>Salon bulunamazsa NotFound, bulunursa Details view'ı döner</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = await _context.Salons
                .Include(s => s.Services)
                .FirstOrDefaultAsync(m => m.SalonID == id);

            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        /// <summary>
        /// Yeni salon ekleme formunu gösterir.
        /// Çalışma günleri seçimi için gerekli verileri ViewBag ile view'a aktarır.
        /// </summary>
        public IActionResult Create()
        {
            ViewBag.WorkingDays = _workingDays;
            return View();
        }

        /// <summary>
        /// Yeni salon kaydı oluşturur.
        /// </summary>
        /// <param name="salon">Oluşturulacak salon bilgileri</param>
        /// <returns>Başarılı ise Index'e yönlendirir, hata varsa formu tekrar gösterir</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalonName,Location,OpeningHours,ClosingHours,WorkingDays")] Salon salon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validate working days
                    if (string.IsNullOrEmpty(salon.WorkingDays))
                    {
                        ModelState.AddModelError("WorkingDays", "En az bir çalışma günü seçmelisiniz.");
                        ViewBag.WorkingDays = _workingDays;
                        return View(salon);
                    }

                    // Validate opening and closing hours
                    if (salon.OpeningHours >= salon.ClosingHours)
                    {
                        ModelState.AddModelError("ClosingHours", "Kapanış saati açılış saatinden sonra olmalıdır.");
                        ViewBag.WorkingDays = _workingDays;
                        return View(salon);
                    }

                    salon.CreatedAt = DateTime.UtcNow;
                    _context.Add(salon);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Salon başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Salon kaydedilirken bir hata oluştu: " + ex.Message);
                }
            }

            ViewBag.WorkingDays = _workingDays;
            return View(salon);
        }

        /// <summary>
        /// Salon düzenleme formunu gösterir.
        /// </summary>
        /// <param name="id">Düzenlenecek salon ID</param>
        /// <returns>Salon bulunamazsa NotFound, bulunursa Edit view'ı döner</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = await _context.Salons.FindAsync(id);
            if (salon == null)
            {
                return NotFound();
            }

            ViewBag.WorkingDays = _workingDays;
            return View(salon);
        }

        /// <summary>
        /// Salon bilgilerini günceller.
        /// </summary>
        /// <param name="id">Güncellenecek salon ID</param>
        /// <param name="salon">Güncellenmiş salon bilgileri</param>
        /// <returns>Başarılı ise Index'e yönlendirir, hata varsa formu tekrar gösterir</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalonID,SalonName,Location,OpeningHours,ClosingHours,WorkingDays")] Salon salon)
        {
            if (id != salon.SalonID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Validate working days
                    if (string.IsNullOrEmpty(salon.WorkingDays))
                    {
                        ModelState.AddModelError("WorkingDays", "En az bir çalışma günü seçmelisiniz.");
                        ViewBag.WorkingDays = _workingDays;
                        return View(salon);
                    }

                    // Validate opening and closing hours
                    if (salon.OpeningHours >= salon.ClosingHours)
                    {
                        ModelState.AddModelError("ClosingHours", "Kapanış saati açılış saatinden sonra olmalıdır.");
                        ViewBag.WorkingDays = _workingDays;
                        return View(salon);
                    }

                    var existingSalon = await _context.Salons.FindAsync(id);
                    if (existingSalon == null)
                    {
                        return NotFound();
                    }

                    // Update only the modifiable properties
                    existingSalon.SalonName = salon.SalonName;
                    existingSalon.Location = salon.Location;
                    existingSalon.OpeningHours = salon.OpeningHours;
                    existingSalon.ClosingHours = salon.ClosingHours;
                    existingSalon.WorkingDays = salon.WorkingDays;

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Salon başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalonExists(salon.SalonID))
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
                    ModelState.AddModelError("", "Salon güncellenirken bir hata oluştu: " + ex.Message);
                }
            }

            ViewBag.WorkingDays = _workingDays;
            return View(salon);
        }

        /// <summary>
        /// Salon silme onay formunu gösterir.
        /// </summary>
        /// <param name="id">Silinecek salon ID</param>
        /// <returns>Salon bulunamazsa NotFound, bulunursa Delete view'ı döner</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salon = await _context.Salons
                .Include(s => s.Services)
                .FirstOrDefaultAsync(m => m.SalonID == id);

            if (salon == null)
            {
                return NotFound();
            }

            return View(salon);
        }

        /// <summary>
        /// Salonu siler.
        /// Eğer salonun hizmetleri varsa silme işlemi engellenir.
        /// </summary>
        /// <param name="id">Silinecek salon ID</param>
        /// <returns>Başarılı ise Index'e yönlendirir, hata varsa Delete view'ı gösterir</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salon = await _context.Salons
                .Include(s => s.Services)
                .FirstOrDefaultAsync(m => m.SalonID == id);

            if (salon == null)
            {
                return NotFound();
            }

            try
            {
                if (salon.Services?.Any() == true)
                {
                    ModelState.AddModelError("", "Bu salon hizmetlere sahip olduğu için silinemez. Önce hizmetleri silmelisiniz.");
                    return View(salon);
                }

                _context.Salons.Remove(salon);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Salon başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Salon silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip salonun var olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="id">Kontrol edilecek salon ID</param>
        /// <returns>Salon varsa true, yoksa false döner</returns>
        private bool SalonExists(int id)
        {
            return _context.Salons.Any(e => e.SalonID == id);
        }
    }
} 