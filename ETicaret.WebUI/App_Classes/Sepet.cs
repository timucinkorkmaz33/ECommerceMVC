using ETicaret.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETicaret.WebUI.App_Classes
{
    public class Sepet
    {
        public static Sepet AktifSepet
        {
            get
            {
                HttpContext ctx = HttpContext.Current;
                if (ctx.Session["AktifSepet"] == null)
                    ctx.Session["AktifSepet"] = new Sepet();
                
                return (Sepet)ctx.Session["AktifSepet"];
            }
        }
        private List<SepetItem> urunler = new List<SepetItem>();

        public List<SepetItem> Urunler
        {
            get { return urunler; }
            set { urunler = value; }
        }
        public void SepeteEkle(SepetItem s)
        {
            if (HttpContext.Current.Session["AktifSepet"] != null)
            {
                Sepet spt = (Sepet)HttpContext.Current.Session["AktifSepet"];
                if (spt.Urunler.Any(x => x.Urun.Id == s.Urun.Id))
                    spt.Urunler.FirstOrDefault(x => x.Urun.Id == s.Urun.Id).Adet++;
                else
                {
                    spt.Urunler.Add(s);
                }
            }
            else
            {
                Sepet spt = new Sepet();
                spt.Urunler.Add(s);
                HttpContext.Current.Session["AktifSepet"] = spt;

            }

           
        }

        public decimal ToplamTutar
        {
            get
            {
                return Urunler.Sum(x => x.Tutar);
            }

        }

    }
    public class SepetItem
    {
        public Urun Urun { get; set; }
        public int Adet { get; set; }
        public double Indirim { get; set; }
        public decimal Tutar
        {
            get
            {
                return Urun.SatisFiyat * Adet * (decimal)(1 - Indirim);
            }
        }
    }
}