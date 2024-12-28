using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KuaforApp.Models;
using KuaforApp.ViewModels;

namespace KuaforApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminAppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminAppointments
        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new AppointmentListViewModel
                {
                    AppointmentID = a.AppointmentID,
                    SalonName = a.Salon.SalonName,
                    ServiceName = a.Service.ServiceName,
                    EmployeeName = a.Employee.FullName,
                    AppointmentDate = a.AppointmentDate,
                    TimeSlot = a.TimeSlot,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return View(appointments);
        }

        // POST: AdminAppointments/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // POST: AdminAppointments/Reject/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: AdminAppointments/Details/5
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

            var viewModel = new AppointmentListViewModel
            {
                AppointmentID = appointment.AppointmentID,
                SalonName = appointment.Salon.SalonName,
                ServiceName = appointment.Service.ServiceName,
                EmployeeName = appointment.Employee.FullName,
                AppointmentDate = appointment.AppointmentDate,
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status,
                CreatedAt = appointment.CreatedAt
            };

            return View(viewModel);
        }
    }
} 