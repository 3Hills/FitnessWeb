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
    }
}