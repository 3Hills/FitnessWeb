using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnessUygulamasi.Controllers
{
    public class HomeController : Controller
    {
        // Dashboard sayfası.
        public ActionResult Index()
        {
            return View();
        }

        // Yeni hareket ekleme sayfası.
        public ActionResult YeniHareketEkle() {
            return View();
        }

        // Yeni antrenman ekleme sayfası.
        public ActionResult YeniAntrenmanEkle() {
            return View();
        }

        // Antrenmana hareket ekleme sayfası.
        public ActionResult AntrenmanaHareketEkle() {
            return View();
        }

        // Harekete set ekleme sayfası.
        public ActionResult HareketeSetEkle() {
            return View();
        }
    }
}