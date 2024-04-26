namespace FrutifyMarket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Messaggi")]
    public partial class Messaggi
    {
        [Key]
        public int IDMessaggio { get; set; }

        public int? FK_ID_Utente { get; set; }

        public int? FK_ID_Ordine { get; set; }

        public string Testo { get; set; }

        public DateTime? Data { get; set; }

        public bool? Ricevuto { get; set; }

        public virtual Users Users { get; set; }

        public virtual Ordini Ordini { get; set; }
    }
}
