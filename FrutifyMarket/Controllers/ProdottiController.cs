using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FrutifyMarket.Controllers
{
    public class ProdottiController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Prodotti
        public ActionResult Index()
        {
            var prodotti = db.Prodotti.Include(p => p.Fornitori);
            return View(prodotti.ToList());
        }

        // GET: Prodotti/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }

        // GET: Prodotti/Create
        public ActionResult Create()
        {
            ViewBag.FK_ID_Fornitore = new SelectList(db.Fornitori, "ID_Fornitore", "RagioneSociale");
            return View();
        }

        // POST: Prodotti/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, 
        // for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Prodotto,Nome,Descrizione,Prezzo,Quantita_Disp,FK_ID_Fornitore,Immagine")] Prodotti prodotti, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Immagini"), fileName);
                    file.SaveAs(path);
                    prodotti.Immagine = "/Content/Immagini/" + fileName;
                }
                else
                {
                    prodotti.Immagine = "/Content/Immagini/default.jpg";
                }
                if (ModelState.IsValid)
                {
                    db.Prodotti.Add(prodotti);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or display a message to the user
                Console.WriteLine("Error saving the file: " + ex.Message);
            }

            ViewBag.FK_ID_Fornitore = new SelectList(db.Fornitori, "ID_Fornitore", "RagioneSociale", prodotti.FK_ID_Fornitore);
            return View(prodotti);
        }

        // GET: Prodotti/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_ID_Fornitore = new SelectList(db.Fornitori, "ID_Fornitore", "RagioneSociale", prodotti.FK_ID_Fornitore);
            return View(prodotti);
        }

        // POST: Prodotti/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, 
        // for more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Prodotto,Nome,Descrizione,Prezzo,Quantita_Disp,FK_ID_Fornitore,Immagine")] Prodotti prodotti, HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Immagini"), fileName);
                    file.SaveAs(path);
                    prodotti.Immagine = "/Content/Immagini/" + fileName;
                }

                if (ModelState.IsValid)
                {
                    db.Entry(prodotti).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or display a message to the user
                Console.WriteLine("Error saving the file: " + ex.Message);
            }

            ViewBag.FK_ID_Fornitore = new SelectList(db.Fornitori, "ID_Fornitore", "RagioneSociale", prodotti.FK_ID_Fornitore);
            return View(prodotti);
        }

        // GET: Prodotti/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotti = db.Prodotti.Find(id);
            if (prodotti == null)
            {
                return HttpNotFound();
            }
            return View(prodotti);
        }

        // POST: Prodotti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prodotti prodotti = db.Prodotti.Find(id);

            // Delete or update the Prodotti_Ordinati records that reference the Prodotti record
            var prodottiOrdinati = db.Prodotti_Ordinati.Where(po => po.FK_ID_Prodotto == id);
            foreach (var po in prodottiOrdinati)
            {
                db.Prodotti_Ordinati.Remove(po);
            }

            db.Prodotti.Remove(prodotti);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Carrello()
        {
            var carrello = Session["Carrello"] as List<Prodotti> ?? new List<Prodotti>();
            return View(carrello);
        }

        public ActionResult AddToCart(int id)
        {
            using (var dbContext = new ModelDBContext())
            {
                var prodotto = dbContext.Prodotti.Find(id);
                if (prodotto != null)
                {
                    var carrello = Session["Carrello"] as List<Prodotti> ?? new List<Prodotti>();

                    // Check if the product is already in the cart
                    if (carrello.Any(p => p.ID_Prodotto == id))
                    {
                        TempData["Message"] = "Errore: Prodotto già presente nel carrello.";
                        return RedirectToAction("Index", "Prodotti");
                    }

                    carrello.Add(prodotto);
                    Session["Carrello"] = carrello;

                    var cookieValue = HttpContext.Request.Cookies["IDCookie"]?.Value;
                    if (!string.IsNullOrEmpty(cookieValue))
                    {
                        // Creazione di un nuovo oggetto Users con il valore del cookie e aggiunta alla lista Utenti
                        var utente = new Users { ID_Utente = Convert.ToInt32(cookieValue) };
                        var utenti = Session["Utenti"] as List<Users> ?? new List<Users>();
                        utenti.Add(utente);
                        Session["Utenti"] = utenti;
                    }
                    else
                    {
                        // Gestione del caso in cui il cookie non ha un valore
                        TempData["Message"] = "Errore: Cookie non presente.";
                        return RedirectToAction("Index", "Prodotti");
                    }

                    // Aggiorna il badge del carrello
                    AggiornaBadgeCarrello();

                    TempData["AddCart"] = "Prodotto aggiunto al carrello con successo.";
                }
                else
                {
                    TempData["Message"] = "Errore: Prodotto non trovato.";
                }

                return RedirectToAction("Index", "Prodotti");
            }
        }

        public ActionResult RemoveFromCart(int id)
        {
            using (var dbContext = new ModelDBContext())
            {
                var carrello = Session["Carrello"] as List<Prodotti>;
                if (carrello != null)
                {
                    // Cerca la pizza nel carrello
                    var pizzaToRemove = carrello.FirstOrDefault(p => p.ID_Prodotto == id);
                    if (pizzaToRemove != null)
                    {
                        // Rimuovi la pizza dal carrello
                        carrello.Remove(pizzaToRemove);
                        Session["Carrello"] = carrello;

                        TempData["CartRemove"] = "Prodotto rimosso dal carrello con successo.";
                        // Aggiorna il badge del carrello
                        AggiornaBadgeCarrello();

                    }
                    else
                    {
                        TempData["Message"] = "Errore: Prodotto non trovato nel carrello.";
                    }
                }
                else
                {
                    TempData["Message"] = "Errore: Carrello non trovato.";
                }

                return RedirectToAction("Carrello", "Prodotti");
            }
        }

        public void AggiornaBadgeCarrello()
        {
            // Ottieni il numero di pizze presenti nella sessione del carrello
            var carrello = Session["Carrello"] as List<Prodotti>;
            var numeroArticoli = carrello?.Count ?? 0;

            // Aggiorna il badge del carrello nella sessione
            Session["BadgeCarrello"] = numeroArticoli;
        }

        public void SvuotaCarrello()
        {
            // Rimuovi tutti gli elementi dal carrello
            Session.Remove("Carrello");

            // Aggiorna il badge del carrello
            AggiornaBadgeCarrello();
        }


        //public ActionResult GetQuantita()
        //{
        //    // Ottieni i dettagli degli ordini in corso
        //    var dettagliOrdiniInCorso = db.Prodotti_Ordinati
        //        .Where(po => po.Stato == "In Corso")
        //        .ToList();

        //    // Itera sui dettagli degli ordini in corso
        //    foreach (var dettaglioOrdine in dettagliOrdiniInCorso)
        //    {
        //        var prodotto = dettaglioOrdine.Prodotti;

        //        // Calcola la quantità da sottrarre (quantità ordinata nel dettaglio dell'ordine)
        //        var quantitaDaSottrarre = dettaglioOrdine.Quantita;

        //        // Aggiorna la quantità disponibile solo per il prodotto ordinato corrente
        //        prodotto.Quantita_Disp -= quantitaDaSottrarre;
        //    }

        //    // Salvataggio delle modifiche nel database
        //    db.SaveChanges();

        //    // Se necessario, puoi selezionare solo i dati che desideri visualizzare nella vista
        //    var prodottiConScorteVisualizzazione = dettagliOrdiniInCorso
        //        .Select(dettaglio => new
        //        {
        //            NomeProdotto = dettaglio.Prodotti.Nome,
        //            NomeFornitore = dettaglio.Prodotti.Fornitori.RagioneSociale,
        //            QuantitaDisponibile = dettaglio.Prodotti.Quantita_Disp
        //        }).ToList();

        //    // Passa questa lista alla vista per la visualizzazione
        //    return View(prodottiConScorteVisualizzazione);
        //}




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
