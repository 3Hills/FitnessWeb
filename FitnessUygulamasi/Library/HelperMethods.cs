using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessUygulamasi.Library
{
    public static class HelperMethods
    {
        public static bool isNumeric(this string value) {
            double sayi;
            return double.TryParse(value, out sayi);
        }
    }
}