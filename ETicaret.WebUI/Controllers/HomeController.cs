using ETicaret.WebUI.App_Classes;
using ETicaret.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ETicaret.WebUI.Controllers
{

    public class HomeController : Controller
    {
        public ContextEticaret ent = new ContextEticaret();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Sepet()
        {
            return PartialView();
        }
        public PartialViewResult Slider()
        {
            var sorgu = ent.Resim.Where(u => u.SliderDurum == true && u.Deleted == false).ToList();
            return PartialView(sorgu);
        }

        public PartialViewResult YeniUrunler()
        {
            var sorgu = ent.Urun.Where(u => u.Deleted == false).ToList();
            return PartialView(sorgu);
        }
        public PartialViewResult Servisler()
        {
            return PartialView();
        }

        public PartialViewResult Markalar()
        {
            var sorgu = ent.Marka.Where(u => u.Deleted == false).ToList();
            return PartialView(sorgu);
        }
        public void SepeteEkle(int Id)
        {
            SepetItem si = new SepetItem();
            Urun u = ent.Urun.Where(x => x.Id == Id).FirstOrDefault();
            si.Urun = u;
            si.Adet = 1;
            si.Indirim = 0;
            Sepet s = new Sepet();
            s.SepeteEkle(si);
            MiniSepet();

        }

        public PartialViewResult MiniSepet()
        {
            if (HttpContext.Session["AktifSepet"] != null)
            {
                return PartialView((Sepet)HttpContext.Session["AktifSepet"]);
            }
            else
            { return PartialView(); }
        }

        public void SepetUrunAdetDusur(int id)
        {
            if (HttpContext.Session["AktifSepet"] != null)
            {
                Sepet s = (Sepet)HttpContext.Session["AktifSepet"];
                if (s.Urunler.FirstOrDefault(x => x.Urun.Id == id).Adet > 1)
                {
                    s.Urunler.FirstOrDefault(x => x.Urun.Id == id).Adet--;

                }
                else
                {
                    SepetItem si = s.Urunler.FirstOrDefault(x => x.Urun.Id == id);
                    s.Urunler.Remove(si);
                }
            }
        }

        public ActionResult UrunDetay(int Id)
        {
            var urun = ent.Urun.FirstOrDefault(u => u.Id == Id);
            var sonuc = ent.UrunOzellik.Where(x => x.UrunId == Id);
            List<OzellikTip> s = new List<OzellikTip>();
            List<OzellikDeger> d = new List<OzellikDeger>();
            var tip = ent.UrunOzellik.Where(x => x.UrunId == Id).GroupBy(u => u.OzellikTipId);
            var deger = ent.UrunOzellik.Where(x => x.UrunId == Id).GroupBy(u => u.OzellikDegerId);
            foreach (var i in tip)
            {
                s.Add(ent.OzellikTip.Where(u => u.Id == i.Key).FirstOrDefault());
            }
            foreach (var i in deger)
            {
               d.Add(ent.OzellikDeger.Where(x => x.Id == i.Key).FirstOrDefault());
            }
            ViewBag.OzellikTip = s;
            ViewBag.OzellikDeger = d;



            return View(urun);
        }
    }
}