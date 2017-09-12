using System;
using System.Linq;
using System.Web.Mvc;
using FitnessUygulamasi.Models;
using FitnessUygulamasi.Library;

namespace FitnessUygulamasi.Controllers
{

    // Bu controllerda CRUD işlemlerini yapıcam.

    public class ActionController : Controller
    {

        FitnessWebAppEntities dbContext = new FitnessWebAppEntities();

        /// <summary>
        /// Yeni hareket kaydı formundan gelen verileri kaydeden method.
        /// </summary>
        /// <param name="hareket"></param>
        /// <returns></returns>
        public ActionResult YeniHareketiKaydet(Hareketler hareket)
        {

            // Önce gelen hareket adı değeri boş mu kontrol ediyorum. Ardından boşluk tuşuyla manipule etmiş mi diye kontrol ediyorum
            // Eğer sıkıntı yoksa 150 karakterden uzun olması durumunu kontrol ediyorum.
            // ##### Bu kontrol işlemlerini daha sonra her form için ayrı ayrı tanımlamak yerine fonksiyona bağlayabilirim.
            if (String.IsNullOrEmpty(hareket.hareketAdi) || String.IsNullOrEmpty(hareket.hareketAdi.Trim()) || hareket.hareketAdi.Length > 150)
            {
                Response.Redirect("/Home/YeniHareketEkle?hata=hareketAdiProblemli");
            }
            else {
                // ########### BU BÖLGEDE HAREKET ADINA GÖRE DAHA ÖNCE EKLENDİ Mİ KONTROLÜ YAPABİLİRİM ###########

                // Hareketler tablosuna veriyi kaydetmesini söylüyorum.
                dbContext.Hareketler.Add(hareket);
                // Kaydetme işlemini yapıyorum.
                dbContext.SaveChanges();
                // Ana sayfaya gönderiyorum.
                Response.Redirect("/Home/Index");
            }

            return View();
        }

        /// <summary>
        /// Yeni antrenman kaydı formundan gelen verileri kaydeden method.
        /// </summary>
        /// <param name="antrenman"></param>
        /// <returns></returns>
        public ActionResult YeniAntrenmaniKaydet(Antrenmanlar antrenman) {

            // Antrenman açıklaması kısmı ile ilgili kontrolleri yapıyorum. Sıkıntı varsa ana sayfaya yönlendiriyorum.
            if (String.IsNullOrEmpty(antrenman.antrenmanAciklama) || String.IsNullOrEmpty(antrenman.antrenmanAciklama.Trim()) || antrenman.antrenmanAciklama.Length > 250)
            {
                Response.Redirect("/Home/YeniAntrenmanEkle?hata=antrenmanAciklamasiProblemli");
            }
            else {

                // Sıkıntı yoksa yeni antrenmanı kaydetmesi için komut veriyorum.
                dbContext.Antrenmanlar.Add(antrenman);
                dbContext.SaveChanges();
                Response.Redirect("/Home/Index");
            }

            return View();
        }

        /// <summary>
        /// Antrenmana yeni hareket ekleme formunda gelen verileri kaydeden method.
        /// </summary>
        /// <param name="antrenmanKayit"></param>
        /// <returns></returns>
        public ActionResult AntrenmanaHareketiKaydet(AntrenmanKayitlari antrenmanKayit) {

            // Antrenmana ait hareketi ekleme komutu veriyorum.
            dbContext.AntrenmanKayitlari.Add(antrenmanKayit);
            // Kayıt işlemini tamamlıyorum.
            dbContext.SaveChanges();
            // Ana sayfaya yolluyorum.
            // ##### Burada şuan eklenen kayıta ait ID numarasını alıp harekete set ekle sayfasına direk yönlendirebilirim.
            Response.Redirect("/Home/Index");

            return View();
        }

        /// <summary>
        /// Hareket kaydına set ekleme formundan gelen verileri kaydeden method.
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public ActionResult HareketeSetiKaydet(HareketSetleri set) {

            // #### Gelen değerler sayı mı değil mi diye kontrol edilecek.
            if (Request.Form["setAgirlik"].isNumeric() == false && Request.Form["setTekrar"].isNumeric() == false)
            {
                Response.Redirect($"/Home/HareketeSetEkle/{Request.Form["kayitID"]}?mesaj=agirlikTekrarSayiDegil");
            }
            else {
                // Harekete seti kaydetmesi komutunu veriyorum.
                dbContext.HareketSetleri.Add(set);
                dbContext.SaveChanges();
                Response.Redirect("/Home/Index");
            }

            return View();
        }

        /// <summary>
        /// Girilen ID'ye ait seti silen method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SetSil(int? id) {

            // Veritabanında gönderilen ID'ye ait seti seçiyorum.
            // Kayıt varsa sil, yoksa geri gönder diyorum.
            var setKontrol = dbContext.HareketSetleri.FirstOrDefault(kayit => kayit.setID == id);
            if (setKontrol != null)
            {
                // Seçilen kaydı Remove fonksiyonuna parametre olarak gönderiyorum.
                dbContext.HareketSetleri.Remove(setKontrol);
                dbContext.SaveChanges();
                Response.Redirect("/Home/Index?mesaj=setSilindi");
            }
            else {
                Response.Redirect("/Home/Index?mesaj=setBulunamadi");
            }

            return View();
        }

        /// <summary>
        /// Antrenmana ait hareketi ve altındaki setleri silen method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AntrenmanKayitKompleSil(int? id) {

            // İlk olarak gelen kayitID'yi aratıcam, varsa ona ait setleri silicem.
            // Setleri silme işlemi tamamlandıktan sonra kayitID'ye ait kaydı silicem.

            var kayitKontrol = dbContext.AntrenmanKayitlari.FirstOrDefault(kayit => kayit.kayitID == id);
            if (kayitKontrol != null)
            {
                // kayitID'ye ait setleri for döngüsü içerisinde listeleyip silicem.
                var setListesi = dbContext.HareketSetleri.Where(set => set.kayitID == id).ToList();
                for (int i = 0; i < setListesi.Count; i++)
                {
                    dbContext.HareketSetleri.Remove(setListesi[i]);
                    dbContext.SaveChanges();
                }
                // Antrenman kaydı silme işlemini döngünün dışında yapıyorum.
                dbContext.AntrenmanKayitlari.Remove(kayitKontrol);
                dbContext.SaveChanges();
                Response.Redirect("/Home/Index?mesaj=hareketVeSetlerSilindi");
            }
            else {
                Response.Redirect("/Home/Index?mesaj=kayitIDBulunamadi");
            }

            return View();
        }

        /// <summary>
        /// Antrenmanı ve altındaki her şeyi silen method.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AntrenmanSil(int? id) {

            // Antrenman altındaki hareketleri, ardından hareket altındaki setleri listeliyorum.
            // Sırasıyla setleri, ardından hareketleri, en sonda da antrenmanı siliyorum.
            var antrenmanKontrol = dbContext.Antrenmanlar.FirstOrDefault(antrenman => antrenman.antrenmanID == id);
            if (antrenmanKontrol != null)
            {
                var kayitListesi = dbContext.AntrenmanKayitlari.Where(kayit => kayit.antrenmanID == antrenmanKontrol.antrenmanID).ToList();
                if (antrenmanKontrol != null) {
                    for (int i = 0; i < kayitListesi.Count; i++)
                    {
                        int seciliKayitID = kayitListesi[i].kayitID; // Linq sorgusunun içine kayitListesi[i].kayitID yazınca hata veriyor.
                        var setListesi = dbContext.HareketSetleri.Where(set => set.kayitID == seciliKayitID).ToList();
                        if (setListesi != null) {
                            for (int z = 0; z < setListesi.Count; z++)
                            {
                                // Burada harekete ait setleri siliyorum.
                                dbContext.HareketSetleri.Remove(setListesi[z]);
                                dbContext.SaveChanges();
                            }
                        }

                        // Burada hareket kaydını silicem.
                        dbContext.AntrenmanKayitlari.Remove(kayitListesi[i]);
                        dbContext.SaveChanges();
                    }
                }

                // Burada antrenmanı silicem.
                dbContext.Antrenmanlar.Remove(antrenmanKontrol);
                dbContext.SaveChanges();
                Response.Redirect("/Home/Index?mesaj=antrenmanBasariylaSilindi");
            }
            else {
                Response.Redirect("/Home/Index?mesaj=antrenmanIDBulunamadi");
            }

            return View();
        }

    }
}