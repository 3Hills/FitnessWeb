using FitnessUygulamasi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUygulamasi.ViewModels
{
    public class WorkoutFullDetail
    {
        public Antreman Ant { get; set; }
        public List<AntremanKayit> AntremanKayitlari { get; set; }
        public Antrenmanlar Antreman { get; internal set; }
    }

    public class AntremanKayit
    {
        public int KayitID { get; set; }
        public int AntremanID { get; set; }
        public int HareketID { get; set; }
        public string HareketAdi { get; set; }
        public int HareketSira { get; set; }
        public List<HareketSetleri> Setler { get; set; }
    }

    public class Antreman
    {
        public int AntrenmanID { get; set; }
        public string AntrenmanAciklama { get; set; }
        public DateTime AntrenmanTarih { get; set; }
        public string AntrenmanDurum { get; set; }

    }
}