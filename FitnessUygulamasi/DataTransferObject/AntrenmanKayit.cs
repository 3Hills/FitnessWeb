using FitnessUygulamasi.Models;
using System.Collections.Generic;

namespace FitnessUygulamasi.DataTransferObject
{
    public class AntrenmanKayit
    {
        public int kayitID { get; set; }
        public int antrenmanID { get; set; }
        public int hareketID { get; set; }
        public string hareketAdi { get; set; }
        public int hareketSira { get; set; }
        public List<HareketSetleri> Setler { get; set; }
    }
}