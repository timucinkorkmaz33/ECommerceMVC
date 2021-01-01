namespace ETicaret.WebUI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MusteriAdres
    {
        public int Id { get; set; }

        public Guid MusteriId { get; set; }

        [Required]
        [StringLength(500)]
        public string Adres { get; set; }

        [Required]
        [StringLength(50)]
        public string AdresAdi { get; set; }

        public virtual Musteri Musteri { get; set; }
    }
}
