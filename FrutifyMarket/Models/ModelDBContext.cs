using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace FrutifyMarket.Controllers
{
    public partial class ModelDBContext : DbContext
    {
        public ModelDBContext()
            : base("name=ModelDBContext")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Documenti> Documenti { get; set; }
        public virtual DbSet<Fornitori> Fornitori { get; set; }
        public virtual DbSet<Messaggi> Messaggi { get; set; }
        public virtual DbSet<Ordini> Ordini { get; set; }
        public virtual DbSet<Prodotti> Prodotti { get; set; }
        public virtual DbSet<Prodotti_Ordinati> Prodotti_Ordinati { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fornitori>()
                .HasMany(e => e.Prodotti)
                .WithOptional(e => e.Fornitori)
                .HasForeignKey(e => e.FK_ID_Fornitore);

            modelBuilder.Entity<Ordini>()
                .Property(e => e.Totale)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Ordini>()
                .HasMany(e => e.Messaggi)
                .WithOptional(e => e.Ordini)
                .HasForeignKey(e => e.FK_ID_Ordine);

            modelBuilder.Entity<Ordini>()
                .HasMany(e => e.Prodotti_Ordinati)
                .WithOptional(e => e.Ordini)
                .HasForeignKey(e => e.FK_ID_Ordine);

            modelBuilder.Entity<Prodotti>()
                .Property(e => e.Prezzo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Prodotti>()
                .HasMany(e => e.Prodotti_Ordinati)
                .WithOptional(e => e.Prodotti)
                .HasForeignKey(e => e.FK_ID_Prodotto);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Documenti)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.FK_ID_Utente);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Messaggi)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.FK_ID_Utente);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Ordini)
                .WithOptional(e => e.Users)
                .HasForeignKey(e => e.FK_ID_Utente);
        }
    }
}
