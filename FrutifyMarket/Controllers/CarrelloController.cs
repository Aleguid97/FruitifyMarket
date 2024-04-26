using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FrutifyMarket.Controllers
{
    public class CarrelloController : Controller
    {
        private ModelDBContext db = new ModelDBContext(); // Definizione del contesto del database

        // GET: Carrello
        public ActionResult Index()
        {
            var cart = Session["cart"] as List<Prodotti>;
            if (cart == null || !cart.Any()) // Check if the cart is empty
            {
                return RedirectToAction("Index", "Prodotti");
            }
            return View(cart);
        }


        public ActionResult Delete(int? id)
        {
            var cart = Session["cart"] as List<Prodotti>;
            if (cart != null)
            {
                var productToRemove = cart.FirstOrDefault(p => p.ID_Prodotto == id);
                if (productToRemove != null)
                {
                    if (productToRemove.Quantita_Disp > 1)
                    {
                        productToRemove.Quantita_Disp--;
                    }
                    else
                    {
                        cart.Remove(productToRemove);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Ordina(string note, string indirizzo)
        {
            var userId = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name)?.ID_Utente;

            var cart = Session["cart"] as List<Prodotti>;
            if (cart != null && cart.Any()) // Check if the cart is not empty
            {
                // Create a new order
                Ordini newOrder = new Ordini();
                newOrder.Data = DateTime.Now;
                newOrder.Stato = "In attesa";
                newOrder.FK_ID_Utente = userId;
                newOrder.Indirizzo = indirizzo;
                newOrder.Totale = cart.Sum(p => p.Prezzo);
                newOrder.Note = note;

                // Save the order to the database
                db.Ordini.Add(newOrder);
                db.SaveChanges();

                // Create a new order detail for each product in the cart
                foreach (var product in cart)
                {
                    Prodotti_Ordinati newDetail = new Prodotti_Ordinati();
                    newDetail.FK_ID_Ordine = newOrder.ID_Ordine;
                    newDetail.FK_ID_Prodotto = product.ID_Prodotto;
                    newDetail.Quantita = Convert.ToInt32(product.Quantita_Disp);

                    // Save the order detail to the database
                    db.Prodotti_Ordinati.Add(newDetail);
                    db.SaveChanges();
                }

                // Clear the cart
                cart.Clear();
            }

            return RedirectToAction("Index", "Prodotti");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(); // Chiamata corretta al metodo Dispose() sul contesto del database
            }
            base.Dispose(disposing);
        }
    }
}
