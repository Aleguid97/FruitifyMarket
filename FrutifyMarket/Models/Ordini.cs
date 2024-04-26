namespace FrutifyMarket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordini")]
    public partial class Ordini
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ordini()
        {
            Messaggi = new HashSet<Messaggi>();
            Prodotti_Ordinati = new HashSet<Prodotti_Ordinati>();
        }

        [Key]
        public int ID_Ordine { get; set; }

        public int? FK_ID_Utente { get; set; }

        public DateTime? Data { get; set; }

        [StringLength(255)]
        public string Indirizzo { get; set; }

        public string Note { get; set; }

        [StringLength(50)]
        public string Stato { get; set; }

        public decimal? Totale { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Messaggi> Messaggi { get; set; }

        public virtual Users Users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prodotti_Ordinati> Prodotti_Ordinati { get; set; }
    }
}
