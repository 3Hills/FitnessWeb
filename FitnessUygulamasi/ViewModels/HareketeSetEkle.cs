using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FitnessUygulamasi.DataTransferObject;

namespace FitnessUygulamasi.ViewModels
{
    public class HareketeSetEkle
    {
        public AntrenmanKayit AntrenmanKayit { get; set; }
        public Antrenman Antrenman { get; set; }
        public Hareket Hareket { get; set; }
    }
}