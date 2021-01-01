using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ETicaret.WebUI.Models;
namespace ETicaret.WebUI.App_Classes
{
    public class Context
    {
        private static ContextEticaret baglanti;

        public static ContextEticaret Baglanti
       {
             get {if (baglanti == null)
                    baglanti = new ContextEticaret();
                return baglanti; }
            set { baglanti = value; }
        }

    }
}