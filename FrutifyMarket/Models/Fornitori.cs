namespace FrutifyMarket.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Fornitori")]
    public partial class Fornitori
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fornitori()
        {
            Prodotti = new HashSet<Prodotti>();
        }

        [Key]
        public int ID_Fornitore { get; set; }

        [StringLength(255)]

        [Required(ErrorMessage = "Il campo Ragione Sociale è obbligatorio")]
        [Display(Name = "Ragione Sociale")]
        public string RagioneSociale { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Il campo P.IVA è obbligatorio")]
        [Display(Name = "P.IVA")]
        public string PIVA { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Il campo Città è obbligatorio")]
        [Display(Name = "Città")]
        public string Citta { get; set; }

        [StringLength(255)]
        public string Indirizzo { get; set; }

        [StringLength(10)]
        public string Cap { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Tel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prodotti> Prodotti { get; set; }
    }
}
