namespace FrutifyMarket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prodotti")]
    public partial class Prodotti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prodotti()
        {
            Prodotti_Ordinati = new HashSet<Prodotti_Ordinati>();
        }

        [Key]
        public int ID_Prodotto { get; set; }

        [StringLength(255)]
        public string Nome { get; set; }

        public string Descrizione { get; set; }

        public decimal Prezzo { get; set; }

        public int? Quantita_Disp { get; set; }

        [Display(Name = "Fornitore")]
        public int? FK_ID_Fornitore { get; set; }

        public string Immagine { get; set; }

        public virtual Fornitori Fornitori { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prodotti_Ordinati> Prodotti_Ordinati { get; set; }
    }
}
