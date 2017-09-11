using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessUygulamasi.Models;
using FitnessUygulamasi.DataTransferObject;

namespace FitnessUygulamasi.Controllers
{
    public class HomeController : Controller
    {

        FitnessWebAppEntities dbContext = new FitnessWebAppEntities();

        /// <summary>
        /// Dashboard sayfası.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var tumAntrenmanlar = (from ant in dbContext.Antrenmanlar.Take(4) // Take fonksiyonu ile 4 kayıt al diyorum.
                                   orderby ant.antrenmanTarih descending
                                   select new AntrenmanListesi
                                   {
                                       antrenmanID = ant.antrenmanID,
                                       antrenmanAciklama = ant.antrenmanAciklama,
                                       antrenmanTarih = ant.antrenmanTarih,
                                       antrenmanDurum = ant.antrenmanDurum
                                   }).ToList();

            return View(tumAntrenmanlar);
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
        public ActionResult AntrenmanaHareketEkle(int id) {

            // Gelen ID'ye ait antrenman var mı kontrol ediyorum. Eğer yoksa ana sayfaya gönderiyorum.
            var antrenmanKontrol = dbContext.Antrenmanlar.Find(id);
                if (antrenmanKontrol == null)
                {
                    Response.Redirect("/Home/Index");
                }

            // Sıkıntı yoksa gelen ID'ye ait kaydın bilgilerini değişkene atıp view kısmına gönderiyorum.
            var antrenmanBilgi = dbContext.Antrenmanlar.Where(antrenman => antrenman.antrenmanID == id).ToList();

            return View(antrenmanBilgi);
        }

        // Harekete set ekleme sayfası.
        public ActionResult HareketeSetEkle(int id) {

            // Gelen ID'ye ait hareket kaydı var mı kontrol ediyorum.
            var kayitKontrol = dbContext.AntrenmanKayitlari.Find(id);
                if (kayitKontrol == null) {
                    Response.Redirect("/Home/Index");
                }

            // ID'ye ait bilgileri değişkene atayıp view dosyasına gönderiyorum.
            var kayitBilgi = dbContext.AntrenmanKayitlari.Where(kayit => kayit.kayitID == id).ToList();

            return View(kayitBilgi);
        }
    }
}