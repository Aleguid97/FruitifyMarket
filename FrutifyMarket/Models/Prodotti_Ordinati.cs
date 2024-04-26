namespace FrutifyMarket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Prodotti_Ordinati
    {
        [Key]
        public int ID_Prodotti_Ordinati { get; set; }

        public int? FK_ID_Ordine { get; set; }

        public int? FK_ID_Prodotto { get; set; }

        public int? Quantita { get; set; }

        public string Stato { get; set; }

        public virtual Ordini Ordini { get; set; }


        public virtual Prodotti Prodotti { get; set; }


    }
}
