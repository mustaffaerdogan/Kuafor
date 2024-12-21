using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProjeAdiniz.Models;

namespace ProjeAdiniz.Controllers
{
    public class KuaforlerController : Controller
    {
        // Mocked data loaded from a file for demonstration purposes
        private static List<Kuafor> kuaforler = LoadKuaforlerFromFile();

        // Method to simulate data loading from a file
        private static List<Kuafor> LoadKuaforlerFromFile()
        {
            // Replace this with actual file reading logic
            return new List<Kuafor>
            {
                new Kuafor { Id = 1, KuaforAdi = "Güzellik Salonu", Adres = "İstanbul", Telefon = "123456789" },
                new Kuafor { Id = 2, KuaforAdi = "Prestij Kuaför", Adres = "Ankara", Telefon = "987654321" },
                new Kuafor { Id = 3, KuaforAdi = "Lüks Kuaför", Adres = "İzmir", Telefon = "456789123" }
            };
        }

        // GET: Kuaforler
        public ActionResult Index()
        {
            return View(kuaforler);
        }

        // GET: Kuaforler/Detay/5
        public ActionResult Detay(int id)
        {
            var kuafor = kuaforler.FirstOrDefault(k => k.Id == id);
            if (kuafor == null)
            {
                return HttpNotFound();
            }
            return View(kuafor);
        }

        // GET: Kuaforler/Yeni
        public ActionResult Yeni()
        {
            return View();
        }

        // POST: Kuaforler/Yeni
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Yeni(Kuafor kuafor)
        {
            if (ModelState.IsValid)
            {
                kuafor.Id = kuaforler.Max(k => k.Id) + 1;
                kuaforler.Add(kuafor);
                return RedirectToAction("Index");
            }
            return View(kuafor);
        }

        // GET: Kuaforler/Duzenle/5
        public ActionResult Duzenle(int id)
        {
            var kuafor = kuaforler.FirstOrDefault(k => k.Id == id);
            if (kuafor == null)
            {
                return HttpNotFound();
            }
            return View(kuafor);
        }

        // POST: Kuaforler/Duzenle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(Kuafor kuafor)
        {
            var mevcutKuafor = kuaforler.FirstOrDefault(k => k.Id == kuafor.Id);
            if (mevcutKuafor == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                mevcutKuafor.KuaforAdi = kuafor.KuaforAdi;
                mevcutKuafor.Adres = kuafor.Adres;
                mevcutKuafor.Telefon = kuafor.Telefon;
                return RedirectToAction("Index");
            }

            return View(kuafor);
        }

        // GET: Kuaforler/Sil/5
        public ActionResult Sil(int id)
        {
            var kuafor = kuaforler.FirstOrDefault(k => k.Id == id);
            if (kuafor == null)
            {
                return HttpNotFound();
            }
            return View(kuafor);
        }

        // POST: Kuaforler/Sil/5
        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken]
        public ActionResult SilOnayla(int id)
        {
            var kuafor = kuaforler.FirstOrDefault(k => k.Id == id);
            if (kuafor == null)
            {
                return HttpNotFound();
            }

            kuaforler.Remove(kuafor);
            return RedirectToAction("Index");
        }
    }
}