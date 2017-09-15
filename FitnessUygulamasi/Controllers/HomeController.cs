using System.Linq;
using System.Web.Mvc;
using FitnessUygulamasi.Models;
using FitnessUygulamasi.ViewModels;

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
            int howManyRowWillShow = id.HasValue ? 8 : 4;

            var allWorkouts = dbContext.Antrenmanlar
                             .OrderByDescending(p => p.antrenmanTarih)
                             .Take(howManyRowWillShow)
                             .Select(p => new WorkoutFullDetail
                             {
                                 Ant = new Antreman

                                 {
                                     AntrenmanID = p.antrenmanID,
                                     AntrenmanAciklama = p.antrenmanAciklama,
                                     AntrenmanTarih = p.antrenmanTarih,
                                     AntrenmanDurum = p.antrenmanDurum
                                 },

                                 AntremanKayitlari = dbContext.AntrenmanKayitlari
                                 .Join(dbContext.Hareketler, ak => ak.hareketID, h => h.hareketID, (ak, h) => new AntremanKayit
                                 {
                                     KayitID = ak.kayitID,
                                     AntremanID = ak.antrenmanID,
                                     HareketID = ak.hareketID,
                                     HareketAdi = h.hareketAdi,
                                     HareketSira = ak.hareketSira,
                                     Setler = dbContext.HareketSetleri.Where(x => x.kayitID == ak.kayitID).ToList()
                                 })
                                 .Where(k => k.AntremanID == p.antrenmanID)
                                 .OrderBy(k => k.KayitID).ToList()
                             })
                             .ToList();
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
        public ActionResult AntrenmanaHareketEkle(int? id) {

            // Gelen ID'ye ait antrenman var mı kontrol ediyorum. Eğer yoksa ana sayfaya gönderiyorum.
            var antrenmanKontrol = dbContext.Antrenmanlar.Find(id);
                if (antrenmanKontrol == null)
                {
                    Response.Redirect("/Home/Index");
                }

            // Sıkıntı yoksa gelen ID'ye ait kaydın bilgilerini değişkene atıp view kısmına gönderiyorum.
            var antrenmanBilgi = dbContext.Antrenmanlar.Where(antrenman => antrenman.antrenmanID == id).ToList();

            // View içerisinde veritabanı sorgusu yapmak yerine controller içerisinden hareket listesini gönderiyorum.
            ViewBag.Hareketler = dbContext.Hareketler.ToList();

            return View(antrenmanBilgi);
        }

        /// <summary>
        /// Harekete set ekleme sayfasını çalıştıran method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult HareketeSetEkle(int? id) {

            // Gelen ID'ye ait hareket kaydı var mı kontrol ediyorum.
            var kayitKontrol = dbContext.AntrenmanKayitlari.Find(id);
                if (kayitKontrol == null) {
                    Response.Redirect("/Home/Index");
                }

            // ID'ye ait bilgileri değişkene atayıp view dosyasına gönderiyorum.
            var kayitBilgi = dbContext.AntrenmanKayitlari.Where(kayit => kayit.kayitID == id).ToList();
            int kayitAntrenmanID = kayitBilgi[0].antrenmanID;
            int kayitHareketID = kayitBilgi[0].hareketID;

            // Kayda ait antrenman ve hareket bilgilerini view içerisine viewbag ile gönderiyorum.
            ViewBag.AntrenmanBilgi = dbContext.Antrenmanlar.Where(antrenman => antrenman.antrenmanID == kayitAntrenmanID).ToList();
            ViewBag.HareketBilgi = dbContext.Hareketler.Where(hareket => hareket.hareketID == kayitHareketID).ToList();

            return View(kayitBilgi);
        }

        /// <summary>
        /// Set güncellemek istendiğinde çalışacak method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SetUpdate(int? id) {

            // Set idsine göre veritabanındaki kaydı seçicem. View içerisine bilgilerini göndericem.
            var setKontrol = dbContext.HareketSetleri.Find(id);
                if (setKontrol == null) {
                    Response.Redirect("/Home/Index?mesaj=setBulunamadi");
                }

            var setBilgi = dbContext.HareketSetleri.Where(set => set.setID == id).ToList();

            return View(setBilgi);
        }
    }
}