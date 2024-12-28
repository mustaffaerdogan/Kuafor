using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KuaforApp.Models;
using KuaforApp.ViewModels;
using System.Security.Claims;

namespace KuaforApp.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments/MyAppointments
        public async Task<IActionResult> MyAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = await _context.Appointments
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .Where(a => a.UserID == userId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Appointments (Admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenBy(a => a.TimeSlot)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.AppointmentID == id);

            if (appointment == null)
            {
                return NotFound();
            }

            // Kullanıcı sadece kendi randevusunu görebilir, admin hepsini görebilir
            if (!User.IsInRole("Admin") && appointment.UserID != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(appointment);
        }

        // POST: Appointments/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Kullanıcı sadece kendi randevusunu iptal edebilir, admin hepsini iptal edebilir
            if (!User.IsInRole("Admin") && appointment.UserID != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            // Geçmiş randevular iptal edilemez
            if (appointment.AppointmentDate < DateTime.UtcNow.Date || 
                (appointment.AppointmentDate == DateTime.UtcNow.Date && 
                TimeSpan.Parse(appointment.TimeSlot) < DateTime.UtcNow.TimeOfDay))
            {
                TempData["ErrorMessage"] = "Geçmiş randevular iptal edilemez.";
                return RedirectToAction(nameof(MyAppointments));
            }

            appointment.Status = AppointmentStatus.Rejected;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Randevunuz başarıyla iptal edildi.";
            return RedirectToAction(nameof(MyAppointments));
        }

        // POST: Appointments/Approve/5 (Admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = AppointmentStatus.Approved;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Randevu başarıyla onaylandı.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Appointments/Reject/5 (Admin only)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = AppointmentStatus.Rejected;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Randevu başarıyla reddedildi.";
            return RedirectToAction(nameof(Index));
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            var viewModel = new AppointmentViewModel
            {
                Salons = new SelectList(_context.Salons, "SalonID", "SalonName")
            };
            return View(viewModel);
        }

        // POST: Appointments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check for existing appointments
                var conflictingAppointment = await _context.Appointments
                    .AnyAsync(a => 
                        a.SalonID == model.SalonID &&
                        a.EmployeeID == model.EmployeeID &&
                        a.AppointmentDate.Date == model.AppointmentDate.Date &&
                        a.TimeSlot == model.TimeSlot &&
                        a.Status != AppointmentStatus.Rejected);

                if (conflictingAppointment)
                {
                    ModelState.AddModelError("", "Bu saat için başka bir randevu bulunmaktadır.");
                    await PrepareDropdownLists(model);
                    return View(model);
                }

                var appointment = new Appointment
                {
                    UserID = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    SalonID = model.SalonID,
                    ServiceID = model.ServiceID,
                    EmployeeID = model.EmployeeID,
                    AppointmentDate = model.AppointmentDate,
                    TimeSlot = model.TimeSlot,
                    Status = AppointmentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Add(appointment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Randevunuz başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }

            await PrepareDropdownLists(model);
            return View(model);
        }

        // AJAX: Get services by salon
        [HttpGet]
        public async Task<JsonResult> GetServicesBySalon(int salonId)
        {
            var services = await _context.Services
                .Where(s => s.SalonID == salonId)
                .Select(s => new { value = s.ServiceID, text = s.ServiceName })
                .ToListAsync();

            return Json(services);
        }

        // AJAX: Get employees by service
        [HttpGet]
        public async Task<JsonResult> GetEmployeesByService(int serviceId)
        {
            var employees = await _context.EmployeeServices
                .Where(es => es.ServiceID == serviceId)
                .Select(es => new { value = es.EmployeeID, text = es.Employee.FullName })
                .ToListAsync();

            return Json(employees);
        }

        // AJAX: Get available time slots
        [HttpGet]
        public async Task<JsonResult> GetAvailableTimeSlots(int employeeId, DateTime date)
        {
            var employee = await _context.Employees
                .Include(e => e.Salon)
                .FirstOrDefaultAsync(e => e.EmployeeID == employeeId);

            if (employee == null)
                return Json(new List<string>());

            // Get working days list
            var workingDays = employee.GetWorkingDaysList();
            var dayOfWeek = date.ToString("dddd", new System.Globalization.CultureInfo("tr-TR"));

            // Check if employee works on selected day
            if (!workingDays.Contains(dayOfWeek))
                return Json(new List<string>());

            // Generate time slots
            var timeSlots = GenerateTimeSlots(employee.OpeningHours, employee.ClosingHours);

            // Get booked appointments
            var bookedSlots = await _context.Appointments
                .Where(a => 
                    a.EmployeeID == employeeId && 
                    a.AppointmentDate.Date == date.Date &&
                    a.Status != AppointmentStatus.Rejected)
                .Select(a => a.TimeSlot)
                .ToListAsync();

            // Remove booked slots
            var availableSlots = timeSlots.Except(bookedSlots).ToList();

            return Json(availableSlots);
        }

        private List<string> GenerateTimeSlots(TimeSpan start, TimeSpan end)
        {
            var slots = new List<string>();
            var current = start;
            var interval = TimeSpan.FromMinutes(30);

            while (current + interval <= end)
            {
                slots.Add(current.ToString(@"hh\:mm"));
                current = current.Add(interval);
            }

            return slots;
        }

        private async Task PrepareDropdownLists(AppointmentViewModel model)
        {
            model.Salons = new SelectList(await _context.Salons.ToListAsync(), "SalonID", "SalonName", model.SalonID);

            if (model.SalonID > 0)
            {
                model.Services = new SelectList(
                    await _context.Services.Where(s => s.SalonID == model.SalonID).ToListAsync(),
                    "ServiceID", "ServiceName", model.ServiceID);
            }

            if (model.ServiceID > 0)
            {
                model.Employees = new SelectList(
                    await _context.EmployeeServices
                        .Where(es => es.ServiceID == model.ServiceID)
                        .Select(es => es.Employee)
                        .ToListAsync(),
                    "EmployeeID", "FullName", model.EmployeeID);
            }
        }
    }
} 