using Microsoft.AspNetCore.Mvc;
using YourNamespace.Models;
using System.Collections.Generic;
using System.Linq;

namespace YourNamespace.Controllers
{
    public class CalisanController : Controller
    {
        // Geçici bir veri kaynağı
        private static List<Calisan> _calisanlar = new List<Calisan>
        {
            new Calisan { Id = 1, Ad = "Ali", Soyad = "Yılmaz", Pozisyon = "Yönetici", Telefon = "1234567890" },
            new Calisan { Id = 2, Ad = "Ayşe", Soyad = "Kara", Pozisyon = "Kuaför", Telefon = "0987654321" }
        };

        // 1. Çalışanların listesi (Index sayfası)
        public IActionResult Index()
        {
            return View(_calisanlar);
        }

        // 2. Çalışan Detayı Görüntüleme
        public IActionResult Detay(int id)
        {
            var calisan = _calisanlar.FirstOrDefault(c => c.Id == id);
            if (calisan == null)
            {
                return NotFound();
            }
            return View(calisan);
        }

        // 3. Yeni Çalışan Ekleme (Form)
        [HttpGet]
        public IActionResult Yeni()
        {
            return View();
        }

        // 4. Yeni Çalışan Ekleme (Veri Gönderimi)
        [HttpPost]
        public IActionResult Yeni(Calisan yeniCalisan)
        {
            if (ModelState.IsValid)
            {
                yeniCalisan.Id = _calisanlar.Any() ? _calisanlar.Max(c => c.Id) + 1 : 1; // Yeni ID oluştur
                _calisanlar.Add(yeniCalisan);
                return RedirectToAction("Index");
            }
            return View(yeniCalisan);
        }

        // 5. Çalışan Düzenleme (Form)
        [HttpGet]
        public IActionResult Duzenle(int id)
        {
            var calisan = _calisanlar.FirstOrDefault(c => c.Id == id);
            if (calisan == null)
            {
                return NotFound();
            }
            return View(calisan);
        }

        // 6. Çalışan Düzenleme (Veri Güncelleme)
        [HttpPost]
        public IActionResult Duzenle(Calisan guncellenmisCalisan)
        {
            if (ModelState.IsValid)
            {
                var calisan = _calisanlar.FirstOrDefault(c => c.Id == guncellenmisCalisan.Id);
                if (calisan == null)
                {
                    return NotFound();
                }

                calisan.Ad = guncellenmisCalisan.Ad;
                calisan.Soyad = guncellenmisCalisan.Soyad;
                calisan.Pozisyon = guncellenmisCalisan.Pozisyon;
                calisan.Telefon = guncellenmisCalisan.Telefon;

                return RedirectToAction("Index");
            }
            return View(guncellenmisCalisan);
        }

        // 7. Çalışan Silme
        [HttpGet]
        public IActionResult Sil(int id)
        {
            var calisan = _calisanlar.FirstOrDefault(c => c.Id == id);
            if (calisan == null)
            {
                return NotFound();
            }
            return View(calisan);
        }

        [HttpPost]
        public IActionResult Sil(Calisan silinecekCalisan)
        {
            var calisan = _calisanlar.FirstOrDefault(c => c.Id == silinecekCalisan.Id);
            if (calisan == null)
            {
                return NotFound();
            }

            _calisanlar.Remove(calisan);
            return RedirectToAction("Index");
        }
    }
}
