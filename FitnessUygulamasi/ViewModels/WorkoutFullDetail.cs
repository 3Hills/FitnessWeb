using FitnessUygulamasi.DataTransferObject;
using System.Collections.Generic;

namespace FitnessUygulamasi.ViewModels
{
    public class WorkoutFullDetail
    {
        public Antrenman Ant { get; set; }
        public List<AntrenmanKayit> AntremanKayitlari { get; set; }
    }
}