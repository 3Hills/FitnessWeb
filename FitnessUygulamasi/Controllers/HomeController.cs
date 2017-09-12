using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessUygulamasi.Models;

namespace FitnessUygulamasi.Controllers
{
    public class HomeController : Controller
    {

        FitnessWebAppEntities dbContext = new FitnessWebAppEntities();

        /// <summary>
        /// Dashboard sayfası.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? id)
        {

            // Eğer ID değeri girilmişse dashboard sayfasında 8 kayıt görüntülenecek.
            int howManyRowWillShow;
            if (id != null)
            {
                howManyRowWillShow = 8;
            }
            else {
                howManyRowWillShow = 4;
            }

            // Eski DataTransferObject yöntemi.
            //var tumAntrenmanlar = (from ant in dbContext.Antrenmanlar.Take(howManyRowWillShow) // Take fonksiyonu ile 4 kayıt al diyorum.
            //                       orderby ant.antrenmanTarih descending
            //                       select new AntrenmanListesi
            //                       {
            //                           antrenmanID = ant.antrenmanID,
            //                           antrenmanAciklama = ant.antrenmanAciklama,
            //                           antrenmanTarih = ant.antrenmanTarih,
            //                           antrenmanDurum = ant.antrenmanDurum
            //                       }).ToList();

            var allWorkouts = dbContext.Antrenmanlar.OrderByDescending(c => c.antrenmanTarih).Take(howManyRowWillShow).ToList();
            for (int i = 0; i < allWorkouts.Count; i++)
            {
                ViewBag.antrenmanKayitlari[i] = dbContext.AntrenmanKayitlari.Where(antrenman => antrenman.antrenmanID == allWorkouts[i].antrenmanID).ToList();
                var seciliHareketID = 0;
                for (int y = 0; y < ViewBag.antrenmanKayitlari[i].Count; y++)
                {
                    seciliHareketID = ViewBag.antrenmanKayitlari[i].hareketID;
                    var hareketBilgi = dbContext.Hareketler.Where(hareket => hareket.hareketID == seciliHareketID).ToList();
                }
            }

            return View(allWorkouts);
        }

        /// <summary>
        /// Yeni hareket ekleme sayfasını çalıştıran method.
        /// </summary>
        /// <returns></returns>
        public ActionResult YeniHareketEkle() {
            return View();
        }

        /// <summary>
        /// Yeni antrenman ekleme sayfasını çalıştıran method.
        /// </summary>
        /// <returns></returns>
        public ActionResult YeniAntrenmanEkle() {
            return View();
        }

        /// <summary>
        /// Antrenmana hareket ekleme sayfasını çalıştıran method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Harekete set ekleme sayfasını çalıştıran method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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