using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using KuaforApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KuaforApp.Controllers
{
    [Authorize]
    public class AIRecommendationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpClientFactory _clientFactory;

        public AIRecommendationController(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            IHttpClientFactory clientFactory)
        {
            _context = context;
            _environment = environment;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("api/AIRecommendation/recommendation")]
        public async Task<IActionResult> GetRecommendation([FromForm] IFormFile photo)
        {
            try
            {
                if (photo == null || photo.Length == 0)
                    return BadRequest("Fotoğraf yüklenmedi.");

                // Create uploads directory if it doesn't exist
                var uploadsDir = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                // Generate unique filename
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine(uploadsDir, fileName);

                // Save the uploaded photo
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                // Send photo to Python AI service
                var client = _clientFactory.CreateClient();
                var formData = new MultipartFormDataContent();
                formData.Add(new StreamContent(System.IO.File.OpenRead(filePath)), "photo", fileName);

                var response = await client.PostAsync("http://localhost:5000/recommendation", formData);
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "AI servisinde bir hata oluştu.");

                var recommendations = await response.Content.ReadAsStringAsync();

                // Save recommendation to database
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var recommendation = new AIRecommendation
                {
                    UserID = userId,
                    PhotoPath = $"/uploads/{fileName}",
                    RecommendedStyles = recommendations,
                    CreatedAt = DateTime.UtcNow
                };

                _context.AIRecommendations.Add(recommendation);
                await _context.SaveChangesAsync();

                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }
    }
} 