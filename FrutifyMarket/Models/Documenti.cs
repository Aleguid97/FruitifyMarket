namespace FrutifyMarket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Documenti")]
    public partial class Documenti
    {
        [Key]
        public int IDDocumento { get; set; }

        [StringLength(50)]
        public string Tipo { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        public int? FK_ID_Utente { get; set; }

        public virtual Users Users { get; set; }
    }
}
