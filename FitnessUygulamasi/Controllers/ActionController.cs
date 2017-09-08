using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FitnessUygulamasi.Models;
using System.Data.Entity;

namespace FitnessUygulamasi.Controllers
{

    // Bu controllerda CRUD işlemlerini yapıcam.

    public class ActionController : Controller
    {

        FitnessWebAppEntities dbContext = new FitnessWebAppEntities();

        // Yeni hareket kaydı yapılan bölüm.
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
                dbContext.Entry(hareket).State = EntityState.Added;
                // Kaydetme işlemini yapıyorum.
                dbContext.SaveChanges();
                // Ana sayfaya gönderiyorum.
                Response.Redirect("/Home/Index");
            }

            return View();
        }

        // Yeni antrenman kaydı yapılan bölüm.
        public ActionResult YeniAntrenmaniKaydet(Antrenmanlar antrenman) {

            // Antrenman açıklaması kısmı ile ilgili kontrolleri yapıyorum. Sıkıntı varsa ana sayfaya yönlendiriyorum.
            if (String.IsNullOrEmpty(antrenman.antrenmanAciklama) || String.IsNullOrEmpty(antrenman.antrenmanAciklama.Trim()) || antrenman.antrenmanAciklama.Length > 250)
            {
                Response.Redirect("/Home/YeniAntrenmanEkle?hata=antrenmanAciklamasiProblemli");
            }
            else {

                // Sıkıntı yoksa yeni antrenmanı kaydetmesi için komut veriyorum.
                dbContext.Entry(antrenman).State = EntityState.Added;
                dbContext.SaveChanges();
                Response.Redirect("/Home/Index");
            }

            return View();
        }

        // Antrenmana hareket kaydedildiğinde çalışacak fonksiyon.
        public ActionResult AntrenmanaHareketiKaydet(AntrenmanKayitlari antrenmanKayit) {

            // Antrenmana ait hareketi ekleme komutu veriyorum.
            dbContext.Entry(antrenmanKayit).State = EntityState.Added;
            // Kayıt işlemini tamamlıyorum.
            dbContext.SaveChanges();
            // Ana sayfaya yolluyorum.
            // ##### Burada şuan eklenen kayıta ait ID numarasını alıp harekete set ekle sayfasına direk yönlendirebilirim.
            Response.Redirect("/Home/Index");

            return View();
        }

        // Hareket kaydına set eklenirken çalışacak fonksiyon.
        public ActionResult HareketeSetiKaydet(HareketSetleri set) {

            // #### Gelen değerler sayı mı değil mi diye kontrol edilecek.
            

            // Harekete seti kaydetmesi komutunu veriyorum.
            dbContext.Entry(set).State = EntityState.Added;
            dbContext.SaveChanges();
            Response.Redirect("/Home/Index");

            return View();
        }

    }
}