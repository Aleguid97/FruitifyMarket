namespace FrutifyMarket.Controllers
{
    using FrutifyMarket.Controllers.Controllo_Campi;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Documenti = new HashSet<Documenti>();
            Messaggi = new HashSet<Messaggi>();
            Ordini = new HashSet<Ordini>();
        }

        [Key]
        public int ID_Utente { get; set; }

        [StringLength(50)]
        //[Nome]
        public string Nome { get; set; }

        [StringLength(50)]
        //[Cognome]
        public string Cognome { get; set; }

        [StringLength(50)]
        //[Username]
        public string Username { get; set; }

        [StringLength(50)]

        [DataType(DataType.Password)]
        //[Password]
        public string Password { get; set; }

        [StringLength(100)]

        public string Email { get; set; }

        [StringLength(50)]
        public string Ruolo { get; set; }

        [StringLength(255)]
        public string Mansione { get; set; }

        [StringLength(16)]
        [Display(Name = "Codice Fiscale")]
        public string CodFisc { get; set; }

        [StringLength(100)]
        [Display(Name = "Città")]
        public string Citta { get; set; }

        [StringLength(255)]
        public string Indirizzo { get; set; }

        [StringLength(10)]
        public string Cap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documenti> Documenti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Messaggi> Messaggi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ordini> Ordini { get; set; }

    }
}
