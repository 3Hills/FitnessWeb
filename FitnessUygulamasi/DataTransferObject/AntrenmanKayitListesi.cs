using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUygulamasi.DataTransferObject
{
    public class AntrenmanKayitListesi
    {
        public int kayitID { get; set; }
        public int antrenmanID { get; set; }
        public int hareketID { get; set; }
        public int hareketSira { get; set; }
    }
}