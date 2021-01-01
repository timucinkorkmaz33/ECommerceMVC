using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ETicaret.WebUI.App_Classes;
using ETicaret.WebUI.Models;

namespace ETicaret.WebUI.Controllers
{
    public class AdminController : Controller
    {
        public ContextEticaret ent = new ContextEticaret();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Urunler()
        {
            var sorgu = ent.Urun.Where(u=>u.Deleted==false).ToList();
            return View(sorgu);

        }
        public ActionResult UrunEkle()
        {
            ViewBag.Kategoriler = ent.Kategori.Where(u=>u.Deleted==false).ToList();
            ViewBag.Markalar = ent.Marka.Where(u=>u.Deleted==false).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult UrunEkle(Urun urn)
        {
            if (ModelState.IsValid)
            {
                urn.EklenmeTarihi = DateTime.Now;
                ent.Urun.Add(urn);
                ent.SaveChanges();
                return RedirectToAction("Urunler");
            }
            return View(urn);
        }
        public ActionResult UrunGuncelle(int Id)
        {
            ViewBag.Kategoriler = ent.Kategori.ToList();
            ViewBag.Markalar = ent.Marka.Where(u => u.Deleted == false).ToList();
            var sorgu = ent.Urun.Where(u => u.Id == Id && u.Deleted==false).FirstOrDefault();
            return View(sorgu);
        }
        [HttpPost]
        public ActionResult UrunGuncelle(Urun urn)
        {
            if (ModelState.IsValid)
            {
                var sorgu = ent.Urun.Where(u => u.Id == urn.Id).FirstOrDefault();
                sorgu.Adi = urn.Adi;
                sorgu.Aciklama = urn.Aciklama;
                sorgu.AlisFiyat = urn.AlisFiyat;
                sorgu.SatisFiyat = urn.SatisFiyat;
                sorgu.MarkaId = urn.MarkaId;
                sorgu.KategoriId = urn.KategoriId;
                ent.SaveChanges();
                return RedirectToAction("Urunler");
            }
            return View(urn);
        }
        public JsonResult UrunSil(int Id)
        {
            var sorgu = ent.Urun.Where(u => u.Id == Id).FirstOrDefault();
            sorgu.Deleted = true;
            ent.SaveChanges();
            var data = ent.Urun.Where(u => u.Deleted == false).ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        public ActionResult Markalar()
        {
            return View(ent.Marka.Where(u=>u.Deleted==false).ToList());
        }
        public ActionResult MarkaEkle()
        {
            Marka m = new Marka();
            return View(m);
        }
        [HttpPost]
        public ActionResult MarkaEkle(Marka m,HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                int resimid = 0;
                if (uploadFile != null)
                {
                    Image img = Image.FromStream(uploadFile.InputStream);
                    int width = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaWidth"].ToString());
                    int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaHeight"].ToString());
                    Bitmap bm = new Bitmap(img, width, height);
                    string name = "/Resim/TumResimler/" + Guid.NewGuid() + Path.GetExtension(uploadFile.FileName);
                    bm.Save(Server.MapPath(name));

                    Resim rs = new Resim();
                    rs.OrtaYol = name;
                    ent.Resim.Add(rs);
                    ent.SaveChanges();
                    resimid = rs.Id;
                }
                if (resimid == 0)
                {
                    ViewBag.Hata = "*Resim Seçilmesi Gereklidir.";
                    return View(m);
                }
                else
                {
                    m.ResimId = resimid;

                }
                ent.Marka.Add(m);
                ent.SaveChanges();

                return RedirectToAction("Markalar");
            }
            else
            {
                ViewBag.Hata = "*Lütfen Tüm Alanları Doldurunuz.";
                return View(m);
            }
        }
        public ActionResult MarkaGuncelle(int Id)
        {
            var sonuc = ent.Marka.Where(u => u.Id == Id && u.Deleted==false).FirstOrDefault();
            return View(sonuc);
        }
        [HttpPost]
        public ActionResult MarkaGuncelle(Marka m, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                int resimid = 0;
                if (uploadFile != null)
                {
                    Image img = Image.FromStream(uploadFile.InputStream);
                    int width = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaWidth"].ToString());
                    int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaHeight"].ToString());
                    Bitmap bm = new Bitmap(img, width, height);
                    string name = "/Resim/TumResimler/" + Guid.NewGuid() + Path.GetExtension(uploadFile.FileName);
                    bm.Save(Server.MapPath(name));

                    Resim rs = ent.Resim.Where(u => u.Id == m.ResimId).FirstOrDefault();
                    rs.OrtaYol = name;
                    ent.SaveChanges();
                    resimid = rs.Id;
                }
                var sorgu = ent.Marka.Where(u => u.Id == m.Id).FirstOrDefault();
                sorgu.Aciklama = m.Aciklama;
                sorgu.Adi = m.Adi;
                if (resimid == 0)
                {
                   
                    ent.SaveChanges();
                    return RedirectToAction("Markalar");
                }
                else
                {
                    m.ResimId = resimid;

                }
                ent.SaveChanges();

                return RedirectToAction("Markalar");
            }
            else
            {
                ViewBag.Hata = "*Lütfen Tüm Alanları Doldurunuz.";
                return View(m);
            }
        }

        public ActionResult MarkaSil(int Id)
        {
            var sorgu = ent.Marka.Where(u => u.Id == Id).FirstOrDefault();
            return View(sorgu);
        }
        [HttpPost]
        public ActionResult MarkaSil(Marka m)
        {
            var sorgu = ent.Marka.Where(u => u.Id == m.Id).FirstOrDefault();
            sorgu.Deleted = true;
            ent.SaveChanges();
            return RedirectToAction("Markalar");
        }

        public ActionResult Kategoriler()
        {
            var sorgu = ent.Kategori.Where(u=>u.Deleted==false).ToList();
            return View(sorgu);
        }

        public ActionResult KategoriEkle()
        {
            Kategori k = new Kategori();
            return View(k);
        }

        [HttpPost]
        public ActionResult KategoriEkle(Kategori k, HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                int resimid = 0;
                if (uploadFile != null)
                {
                    Image img = Image.FromStream(uploadFile.InputStream);
                    int width = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaWidth"].ToString());
                    int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaHeight"].ToString());
                    Bitmap bm = new Bitmap(img, width, height);
                    string name = "/Resim/TumResimler/" + Guid.NewGuid() + Path.GetExtension(uploadFile.FileName);
                    bm.Save(Server.MapPath(name));

                    Resim rs = new Resim();
                    rs.OrtaYol = name;
                    ent.Resim.Add(rs);
                    ent.SaveChanges();
                    resimid = rs.Id;
                }
                if (resimid == 0)
                {
                    ViewBag.Hata = "*Resim Seçilmesi Gereklidir.";
                    return View(k);
                }
                else
                {
                    k.ResimId = resimid;

                }
                ent.Kategori.Add(k);
                ent.SaveChanges();

                return RedirectToAction("Kategoriler");
            }
            else
            {
                ViewBag.Hata = "*Lütfen Tüm Alanları Doldurunuz.";
                return View(k);
            }
        }
        public ActionResult KategoriGuncelle(int Id)
        {
            var sorgu = ent.Kategori.Where(u => u.Id == Id && u.Deleted==false).FirstOrDefault();
            return View(sorgu);

        }
        [HttpPost]
        public ActionResult KategoriGuncelle(Kategori k,HttpPostedFileBase uploadFile)
        {
            if (ModelState.IsValid)
            {
                int resimid = 0;
                if (uploadFile != null)
                {
                    Image img = Image.FromStream(uploadFile.InputStream);
                    int width = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaWidth"].ToString());
                    int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaHeight"].ToString());
                    Bitmap bm = new Bitmap(img, width, height);
                    string name = "/Resim/TumResimler/" + Guid.NewGuid() + Path.GetExtension(uploadFile.FileName);
                    bm.Save(Server.MapPath(name));

                    Resim rs = ent.Resim.Where(u => u.Id == k.ResimId).FirstOrDefault();
                    rs.OrtaYol = name;
                    ent.SaveChanges();
                    resimid = rs.Id;
                }
                var sorgu = ent.Kategori.Where(u => u.Id == k.Id).FirstOrDefault();
                sorgu.Aciklama = k.Aciklama;
                sorgu.Adi = k.Adi;
                if (resimid == 0)
                {

                    ent.SaveChanges();
                    return RedirectToAction("Kategoriler");
                }
                else
                {
                    k.ResimId = resimid;

                }
                ent.SaveChanges();

                return RedirectToAction("Kategoriler");
            }
            else
            {
                ViewBag.Hata = "*Lütfen Tüm Alanları Doldurunuz.";
                return View(k);
            }
        }
        public ActionResult KategoriSil(int Id)
        {
            var sorgu = ent.Marka.Where(u => u.Id == Id).FirstOrDefault();
            return View(sorgu);
        }
        [HttpPost]
        public ActionResult KategoriSil(Kategori k)
        {
            var sorgu = ent.Kategori.Where(u => u.Id == k.Id).FirstOrDefault();
            sorgu.Deleted = true;
            ent.SaveChanges();
            return RedirectToAction("Kategoriler");
        }

        public ActionResult OzellikTipleri()
        {
            var sorgu = ent.OzellikTip.Where(u=>u.Deleted==false).ToList();
            return View(sorgu);
        }

        public ActionResult OzellikTipEkle()
        {
            OzellikTip ot = new OzellikTip();
            ViewBag.Kategoriler = ent.Kategori.Where(u => u.Deleted == false).ToList();
            return View(ot);
        }
        [HttpPost]
        public ActionResult OzellikTipEkle(OzellikTip ot)
        {
            if (ModelState.IsValid)
            {
                ent.OzellikTip.Add(ot);
                ent.SaveChanges();
                return RedirectToAction("OzellikTipleri");
            }
            ViewBag.Hata = "Tüm Alanların Girilmesi Gereklidir.";
            return View(ot);
        }
        public ActionResult OzellikTipGuncelle(int Id)
        {
            OzellikTip ot = ent.OzellikTip.Where(u => u.Id == Id && u.Deleted==false).FirstOrDefault();
            ViewBag.Kategoriler = ent.Kategori.Where(u => u.Deleted == false).ToList();
            return View(ot);
        }
        [HttpPost]
        public ActionResult OzellikTipGuncelle(OzellikTip ot)
        {
            if (ModelState.IsValid)
            {
                var sorgu = ent.OzellikTip.Where(u => u.Id == ot.Id).FirstOrDefault();
                sorgu.KategoriId = ot.KategoriId;
                sorgu.Aciklama = ot.Aciklama;
                sorgu.Adi = ot.Adi;
                ent.SaveChanges();
                return RedirectToAction("OzellikTipleri");
            }
            ViewBag.Hata = "Tüm Alanların Girilmesi Gereklidir.";
            return View(ot);
        }
        public ActionResult OzellikTipSil(int Id)
        {
            var sorgu = ent.OzellikTip.Where(u => u.Id == Id).FirstOrDefault();
            sorgu.Deleted = true;
            ent.SaveChanges();
            return RedirectToAction("OzellikTipleri");
        }

        public ActionResult OzellikDegerleri()
        {
            var sorgu = ent.OzellikDeger.Where(u => u.Deleted == false).ToList();
            return View(sorgu);
        }

        public ActionResult OzellikDegerEkle()
        {
            OzellikDeger od = new OzellikDeger();
            ViewBag.OzellikTip = ent.OzellikTip.Where(u => u.Deleted == false).ToList();
            return View(od);
        }
        [HttpPost]
        public ActionResult OzellikDegerEkle(OzellikDeger od)
        {
            if (ModelState.IsValid)
            {
                ent.OzellikDeger.Add(od);
                ent.SaveChanges();
                return RedirectToAction("OzellikDegerleri");
            }
            ViewBag.Hata = "Tüm Alanların Girilmesi Gereklidir.";
            return View(od);
        }
        public ActionResult OzellikDegerGuncelle(int Id)
        {
            OzellikDeger od = ent.OzellikDeger.Where(u => u.Id == Id).FirstOrDefault();
            ViewBag.OzellikTip = ent.OzellikTip.Where(u => u.Deleted == false).ToList();
            return View(od);
        }
        [HttpPost]
        public ActionResult OzellikDegerGuncelle(OzellikDeger od)
        {
            if (ModelState.IsValid)
            {
                var sorgu = ent.OzellikDeger.Where(u => u.Id == od.Id).FirstOrDefault();
                sorgu.OzellikTipId = od.OzellikTipId;
                sorgu.Aciklama = od.Aciklama;
                sorgu.Adi = od.Adi;
                ent.SaveChanges();
                return RedirectToAction("OzellikDegerleri");
            }
            ViewBag.Hata = "Tüm Alanların Girilmesi Gereklidir.";
            return View(od);
        }
        public ActionResult OzellikDegerSil(int Id)
        {
            var sorgu = ent.OzellikDeger.Where(u => u.Id == Id).FirstOrDefault();
            sorgu.Deleted = true;
            ent.SaveChanges();
            return RedirectToAction("OzellikDegerleri");
        }

        public ActionResult UrunOzellikleri()
        {
            var sorgu = ent.UrunOzellik.Where(u => u.Deleted == false).ToList();
            return View(sorgu);
        }

        public ActionResult UrunOzellikEkle()
        {
            UrunOzellik ur = new UrunOzellik();
            ViewBag.Urunler = ent.Urun.Where(u => u.Deleted == false).ToList();
            return View(ur);
        }
        [HttpPost]
        public ActionResult UrunOzellikEkle(UrunOzellik ur)
        {
            if (ModelState.IsValid)
            {
                UrunOzellik urn = new UrunOzellik();
                urn.OzellikDegerId = ur.OzellikDegerId;
                urn.OzellikTipId = ur.OzellikTipId;
                urn.UrunId = ur.UrunId;
                ent.UrunOzellik.Add(urn);
                ent.SaveChanges();
                return RedirectToAction("UrunOzellikleri");
            }
            return View(ur);
        }

        public ActionResult UrunOzellikGuncelle(int Id)
        {
            var sorgu = ent.UrunOzellik.Where(u => u.Id==Id).FirstOrDefault();
            ViewBag.Urunler = ent.Urun.Where(u => u.Deleted == false).ToList();
            return View(sorgu);
        }
        [HttpPost]
        public ActionResult UrunOzellikGuncelle(UrunOzellik urn)
        {
            var sorgu = ent.UrunOzellik.Where(u => u.Id == urn.Id).FirstOrDefault();
            sorgu.UrunId = urn.UrunId;
            sorgu.OzellikTipId = urn.OzellikTipId;
            sorgu.OzellikDegerId = urn.OzellikDegerId;
            ent.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        }
        public ActionResult UrunOzellikSil(int Id)
        {
            var sorgu = ent.UrunOzellik.Where(u => u.Id == Id).FirstOrDefault();
            sorgu.Deleted = true;
            ent.SaveChanges();
            return RedirectToAction("UrunOzellikleri");
        }
        public PartialViewResult OzellikTipPartial(int? KId)
        {
            var sorgu = new List<OzellikTip>();
            if (KId != null)
            {
                 sorgu = ent.OzellikTip.Where(u => u.Deleted == false && u.KategoriId == KId).ToList();
            }
            else
            {
                sorgu = ent.OzellikTip.Where(u => u.Deleted == false).ToList();
            }
            return PartialView(sorgu);
        }
        public PartialViewResult OzellikDegerPartial(int? OId)
        {
            var sorgu = new List<OzellikDeger>();
            if (OId != null)
            {
                 sorgu = ent.OzellikDeger.Where(u => u.Deleted == false && u.OzellikTipId == OId).ToList();
            }
            else
            {
                 sorgu = ent.OzellikDeger.Where(u => u.Deleted == false).ToList();

            }
            return PartialView(sorgu);
        }

        public ActionResult UrunResimEkle(int Id)
        {
           
            return View(Id);
        }
        [HttpPost]
        public ActionResult UrunResimEkle(int uId,HttpPostedFileBase resim)
        {
            if (resim != null)
            {
                Image img = Image.FromStream(resim.InputStream);
                Bitmap ortaresim = new Bitmap(img, Settings.UrunOrtaBoyut);
                Bitmap buyukresim = new Bitmap(img, Settings.UrunBuyukBoyut);
                string ortaYol = "/Resim/Orta/" + Guid.NewGuid()+Path.GetExtension(resim.FileName);
                string buyukYol = "/Resim/Buyuk/" + Guid.NewGuid()+Path.GetExtension(resim.FileName);
                ortaresim.Save(Server.MapPath(ortaYol));
                buyukresim.Save(Server.MapPath(buyukYol));

                Resim rs = new Resim();
                rs.BuyukYol = buyukYol;
                rs.OrtaYol = ortaYol;
                rs.UrunId = uId;
                var VarsayilanSorgula = ent.Resim.Where(u => u.UrunId == uId).ToList();
                var count = VarsayilanSorgula.Where(u => u.Varsayilan == true).Count();
                if (count<=0)
                {
                    rs.Varsayilan = true;
                }
                else { rs.Varsayilan = false; }
                ent.Resim.Add(rs);
                ent.SaveChanges();
                return View(uId);
            }
            return View(uId);
        }

        public ActionResult SliderList()
        {
            var sorgu = ent.Resim.Where(u => u.SliderDurum == true && u.Deleted == false).ToList();
            return View(sorgu);
        }

        public ActionResult SliderResimEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SliderResimEkle(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);
                Bitmap bmp = new Bitmap(img, Settings.SliderResimBoyut);
                string yol = "/Resim/SliderResim/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);
                bmp.Save(Server.MapPath(yol));
                Resim rs = new Resim();
                rs.BuyukYol = yol;
                rs.SliderDurum = true;
                ent.Resim.Add(rs);
                ent.SaveChanges();
            }
            return RedirectToAction("SliderList");
        }

        public JsonResult SliderResimSil(int Id)
        {
            var sorgu = ent.Resim.Where(u => u.Id == Id).FirstOrDefault();
            sorgu.Deleted = true;
            ent.SaveChanges();
            var data = ent.Resim.Where(u => u.Deleted == false && u.SliderDurum==true).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}