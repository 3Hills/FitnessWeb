using System.Linq;
using System.Web.Mvc;
using FitnessUygulamasi.Models;
using FitnessUygulamasi.ViewModels;
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
        public ActionResult Index(int? id)
        {
            // Eğer ID değeri girilmişse dashboard sayfasında 8 kayıt görüntülenecek.
            int howManyRowWillShow = id.HasValue ? 8 : 4;

            var allWorkouts = dbContext.Antrenmanlar
                             .OrderByDescending(p => p.antrenmanTarih)
                             .Take(howManyRowWillShow)
                             .Select(p => new WorkoutFullDetail
                             {
                                 Ant = new Antrenman

                                 {
                                     antrenmanID = p.antrenmanID,
                                     antrenmanAciklama = p.antrenmanAciklama,
                                     antrenmanTarih = p.antrenmanTarih,
                                     antrenmanDurum = p.antrenmanDurum
                                 },

                                 AntremanKayitlari = dbContext.AntrenmanKayitlari
                                 .Join(dbContext.Hareketler, ak => ak.hareketID, h => h.hareketID, (ak, h) => new AntrenmanKayit
                                 {
                                     kayitID = ak.kayitID,
                                     antrenmanID = ak.antrenmanID,
                                     hareketID = ak.hareketID,
                                     hareketAdi = h.hareketAdi,
                                     hareketSira = ak.hareketSira,
                                     Setler = dbContext.HareketSetleri.Where(x => x.kayitID == ak.kayitID).ToList()
                                 })
                                 .Where(k => k.antrenmanID == p.antrenmanID)
                                 .OrderBy(k => k.hareketSira).ToList()
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

            var antrenmanaHareketEkle = dbContext.Antrenmanlar
                        // Gönderilen idye ait antrenmanı seçtiriyorum.
                        .Where(k => k.antrenmanID == id)
                        // Seçilen veri AntrenmanaHareketEkle tipinde olacak.
                        .Select(ant => new AntrenmanaHareketEkle {

                            // AntrenmanaHareketEkle sınıfındaki Antrenman özelliğini Antrenman sınıfından türetiyorum.
                            // Antrenman özelliğinin tipi 'Antrenman' olduğundan (List<Antrenman> değil) ToList() koymuyorum.
                            Antrenman = new Antrenman {
                                antrenmanID = ant.antrenmanID,
                                antrenmanAciklama = ant.antrenmanAciklama,
                                antrenmanTarih = ant.antrenmanTarih,
                                antrenmanDurum = ant.antrenmanDurum
                            },
                            // AntrenmanaHareketEkle sınıfındaki Hareketler özelliğini Hareket sınıfından türetiyorum.
                            // Hareketler özelliğinin tipi List<Hareket> olduğundan sonuna ToList() koyuyorum.
                            Hareketler = dbContext.Hareketler
                                            .Select(hrkt => new Hareket {
                                                hareketID = hrkt.hareketID,
                                                hareketAdi = hrkt.hareketAdi
                                            }).ToList()

                        }).ToList();

            return View(antrenmanaHareketEkle);
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

            var hareketeSetEkle = dbContext.AntrenmanKayitlari
                        .Where(a => a.kayitID == id)
                        .Select(b => new HareketeSetEkle {

                            AntrenmanKayit = new AntrenmanKayit {
                                kayitID = b.kayitID,
                                antrenmanID = b.antrenmanID,
                                hareketID = b.hareketID,
                                hareketSira = b.hareketSira
                            },

                            Antrenman = dbContext.Antrenmanlar.Where(c => c.antrenmanID == b.antrenmanID)
                                        .Select(d => new Antrenman {
                                            antrenmanAciklama = d.antrenmanAciklama,
                                            antrenmanTarih = d.antrenmanTarih,
                                            antrenmanDurum = d.antrenmanDurum
                                        }).FirstOrDefault(),
                        
                            Hareket = dbContext.Hareketler.Where(e => e.hareketID == b.hareketID)
                                      .Select(f => new Hareket {
                                          hareketAdi = f.hareketAdi
                                      }).FirstOrDefault()

                        })
                        .ToList();

            return View(hareketeSetEkle);
        }

        /// <summary>
        /// Set güncelleme formu görüntülenmek istendiğinde çalışacak method.
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