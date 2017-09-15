using FitnessUygulamasi.DataTransferObject;
using System.Collections.Generic;

namespace FitnessUygulamasi.ViewModels
{
    public class AntrenmanaHareketEkle
    {
        public Antrenman Antrenman { get; set; }
        public List<Hareket> Hareketler { get; set; }
    }
}