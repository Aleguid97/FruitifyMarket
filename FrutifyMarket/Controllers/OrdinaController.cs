using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrutifyMarket.Controllers
{
    public class OrdinaController : Controller
    {
        // GET: Ordina
        public ActionResult Index()
        {
            return View();
        }

        private void AggiornaBadgeCarrello()
        {
            var carrello = Session["Carrello"] as List<Prodotti>;
            var numeroArticoli = carrello?.Count ?? 0;

            Session["BadgeCarrello"] = numeroArticoli;
        }

        [HttpPost]
        public ActionResult Ordina(string note, string indirizzo, int quantita, int costotot, string stato)
        {
            ModelDBContext db = new ModelDBContext();
            int userId = db.Users.FirstOrDefault(u => u.Username == User.Identity.Name)?.ID_Utente ?? 0;

            if (userId != 0 && Session["Carrello"] is List<Prodotti> cart && cart.Any())
            {
                // Crea un nuovo ordine principale
                Ordini newOrder = new Ordini();
                newOrder.FK_ID_Utente = userId;
                newOrder.Data = DateTime.Now.Date; // Remove the time component from the date
                newOrder.Indirizzo = indirizzo;
                newOrder.Totale = 0; // Inizializza il totale dell'ordine a 0
                newOrder.Note = note;
                newOrder.Stato = stato; // Modifica lo stato come preferisci

                // Aggiungi l'ordine principale al database
                db.Ordini.Add(newOrder);
                db.SaveChanges();

                // Associa i dettagli dell'ordine all'ordine principale e calcola il totale dell'ordine principale
                foreach (var prodotto in cart)
                {
                    // Recupera il prodotto dal database
                    var prodottoDalDatabase = db.Prodotti.FirstOrDefault(p => p.ID_Prodotto == prodotto.ID_Prodotto);

                    if (prodottoDalDatabase != null)
                    {
                        // Verifica se la quantità disponibile è sufficiente per soddisfare l'ordine
                        if (prodottoDalDatabase.Quantita_Disp >= quantita)
                        {
                            // Sottrae la quantità ordinata dalla quantità disponibile
                            prodottoDalDatabase.Quantita_Disp -= quantita;

                            // Aggiorna il database
                            db.Entry(prodottoDalDatabase).State = EntityState.Modified;
                            db.SaveChanges();

                            // Aggiungi il dettaglio dell'ordine al database
                            Prodotti_Ordinati prodottoOrdinato = new Prodotti_Ordinati();
                            prodottoOrdinato.FK_ID_Ordine = newOrder.ID_Ordine;
                            prodottoOrdinato.FK_ID_Prodotto = prodotto.ID_Prodotto;
                            prodottoOrdinato.Quantita = quantita;
                            prodottoOrdinato.Stato = stato; // Aggiungi questa linea per impostare lo stato
                            db.Prodotti_Ordinati.Add(prodottoOrdinato);

                            // Aggiungi il prezzo del prodotto al totale dell'ordine principale
                            newOrder.Totale += costotot; // Modifica questa linea per aggiungere al totale anziché sovrascriverlo
                        }
                        else
                        {
                            // Gestisci il caso in cui la quantità disponibile non è sufficiente
                            // Ad esempio, puoi informare l'utente o gestire l'ordine in modo diverso
                        }
                    }
                    else
                    {
                        // Gestisci il caso in cui il prodotto non sia stato trovato nel database
                        // Ad esempio, puoi informare l'utente o gestire l'ordine in modo diverso
                    }
                }

                // Salva le modifiche al totale dell'ordine principale
                db.SaveChanges();

                // Cancella i dettagli dell'ordine dal carrello e dalla sessione
                var idProdottiDaRimuovere = cart.Select(p => p.ID_Prodotto).ToList();
                var dettagliOrdineDaRimuovere = db.Prodotti_Ordinati.Where(po => idProdottiDaRimuovere.Contains((int)po.FK_ID_Prodotto)).ToList();

                // Svuota il carrello e aggiorna la sessione
                cart.Clear();
                Session["Carrello"] = cart;

                // Aggiorna il badge del carrello
                AggiornaBadgeCarrello();

                TempData["OrderConfirm"] = "Ordine effettuato con successo!";
            }

            return RedirectToAction("GetQuantita", "Prodotti");
        }


    }
}