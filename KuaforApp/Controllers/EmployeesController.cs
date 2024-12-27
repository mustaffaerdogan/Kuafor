using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KuaforApp.Models;
using KuaforApp.ViewModels;

namespace KuaforApp.Controllers
{
    [Authorize] // Temel yetkilendirme - tüm kullanıcılar için
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees - Tüm kullanıcılar erişebilir
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Salon)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.Service)
                .ToListAsync();

            // Admin olmayan kullanıcılar için düzenleme butonlarını gizlemek için ViewBag kullanılır
            ViewBag.IsAdmin = User.IsInRole("Admin");
            return View(employees);
        }

        // GET: Employees/Create - Sadece Admin erişebilir
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = new EmployeeCreateViewModel
            {
                SalonList = new SelectList(_context.Salons, "SalonID", "SalonName")
            };
            return View(viewModel);
        }

        // POST: Employees/Create - Sadece Admin erişebilir
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var salon = await _context.Salons
                    .Include(s => s.Services)
                    .FirstOrDefaultAsync(s => s.SalonID == model.SalonID);

                if (salon == null)
                {
                    ModelState.AddModelError("", "Seçilen salon bulunamadı.");
                    model.SalonList = new SelectList(_context.Salons, "SalonID", "SalonName");
                    return View(model);
                }

                var employee = new Employee
                {
                    FullName = model.FullName,
                    SalonID = salon.SalonID,
                    WorkingDays = salon.WorkingDays,
                    OpeningHours = salon.OpeningHours,
                    ClosingHours = salon.ClosingHours,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                foreach (var service in salon.Services)
                {
                    var employeeService = new EmployeeService
                    {
                        EmployeeID = employee.EmployeeID,
                        ServiceID = service.ServiceID,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.EmployeeServices.Add(employeeService);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Çalışan başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            model.SalonList = new SelectList(_context.Salons, "SalonID", "SalonName");
            return View(model);
        }

        // GET: Employees/Edit/5 - Sadece Admin erişebilir
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Salon)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync(e => e.EmployeeID == id);

            if (employee == null)
            {
                return NotFound();
            }

            var viewModel = new EmployeeEditViewModel
            {
                EmployeeID = employee.EmployeeID,
                FullName = employee.FullName,
                SalonID = employee.SalonID,
                WorkingDays = employee.WorkingDays,
                OpeningHours = employee.OpeningHours,
                ClosingHours = employee.ClosingHours,
                SalonList = new SelectList(_context.Salons, "SalonID", "SalonName"),
                CurrentServices = employee.EmployeeServices.Select(es => es.Service).ToList()
            };

            return View(viewModel);
        }

        // POST: Employees/Edit/5 - Sadece Admin erişebilir
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, EmployeeEditViewModel model)
        {
            if (id != model.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _context.Employees
                        .Include(e => e.EmployeeServices)
                        .FirstOrDefaultAsync(e => e.EmployeeID == id);

                    if (employee == null)
                    {
                        return NotFound();
                    }

                    var salon = await _context.Salons
                        .Include(s => s.Services)
                        .FirstOrDefaultAsync(s => s.SalonID == model.SalonID);

                    if (salon == null)
                    {
                        ModelState.AddModelError("", "Seçilen salon bulunamadı.");
                        model.SalonList = new SelectList(_context.Salons, "SalonID", "SalonName");
                        return View(model);
                    }

                    employee.FullName = model.FullName;
                    employee.SalonID = salon.SalonID;
                    employee.WorkingDays = salon.WorkingDays;
                    employee.OpeningHours = salon.OpeningHours;
                    employee.ClosingHours = salon.ClosingHours;

                    _context.EmployeeServices.RemoveRange(employee.EmployeeServices);

                    foreach (var service in salon.Services)
                    {
                        var employeeService = new EmployeeService
                        {
                            EmployeeID = employee.EmployeeID,
                            ServiceID = service.ServiceID,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.EmployeeServices.Add(employeeService);
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Çalışan başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(model.EmployeeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            model.SalonList = new SelectList(_context.Salons, "SalonID", "SalonName");
            return View(model);
        }

        // GET: Employees/Delete/5 - Sadece Admin erişebilir
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Salon)
                .Include(e => e.EmployeeServices)
                    .ThenInclude(es => es.Service)
                .FirstOrDefaultAsync(m => m.EmployeeID == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5 - Sadece Admin erişebilir
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.EmployeeServices)
                .FirstOrDefaultAsync(e => e.EmployeeID == id);

            if (employee == null)
            {
                return NotFound();
            }

            _context.EmployeeServices.RemoveRange(employee.EmployeeServices);
            _context.Employees.Remove(employee);
            
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Çalışan başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        // AJAX endpoint to get salon details - Tüm kullanıcılar erişebilir
        [HttpGet]
        public async Task<IActionResult> GetSalonDetails(int salonId)
        {
            var salon = await _context.Salons
                .Include(s => s.Services)
                .FirstOrDefaultAsync(s => s.SalonID == salonId);

            if (salon == null)
                return NotFound();

            return Json(new
            {
                workingDays = salon.WorkingDays,
                openingHours = salon.OpeningHours.ToString(@"hh\:mm"),
                closingHours = salon.ClosingHours.ToString(@"hh\:mm"),
                services = salon.Services.Select(s => new
                {
                    id = s.ServiceID,
                    name = s.ServiceName,
                    price = s.Price
                }).ToList()
            });
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }
    }
} 