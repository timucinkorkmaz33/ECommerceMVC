namespace ETicaret.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UrunOzellik")]
    public partial class UrunOzellik
    {
        public int Id { get; set; }

        public int UrunId { get; set; }

        public int OzellikTipId { get; set; }

        public int OzellikDegerId { get; set; }

        public bool Deleted { get; set; }

        public virtual OzellikDeger OzellikDeger { get; set; }

        public virtual OzellikTip OzellikTip { get; set; }

        public virtual Urun Urun { get; set; }
    }
}
