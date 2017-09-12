using System;

namespace FitnessUygulamasi.DataTransferObject
{
    public class AntrenmanListesi
    {
        public int antrenmanID { get; set; }
        public string antrenmanAciklama { get; set; }
        public DateTime antrenmanTarih { get; set; }
        public string antrenmanDurum { get; set; }
    }
}