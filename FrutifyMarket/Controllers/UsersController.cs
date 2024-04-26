using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace FrutifyMarket.Controllers
{
    public class UsersController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Utente,Nome,Cognome,Username,Password,Email,Ruolo,Mansione,CodFisc,Citta,Indirizzo,Cap")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Utente,Nome,Cognome,Username,Password,Email,Ruolo,Mansione,CodFisc,Citta,Indirizzo,Cap")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult GetUserDetails()
        {
            // Ottieni l'ID dell'utente loggato dal cookie
            int id;
            if (Request.Cookies["IDCookie"] != null && int.TryParse(Request.Cookies["IDCookie"].Value, out id))
            {
                // Trova l'utente nel database utilizzando l'ID
                var user = db.Users.AsNoTracking().FirstOrDefault(u => u.ID_Utente == id);

                if (user != null)
                {
                    // Crea un elenco che contenga il singolo utente
                    var userList = new List<Users> { user };

                    // Passa l'elenco alla vista
                    return View(userList);
                }
            }

            // Se non è possibile ottenere l'ID dell'utente dal cookie o l'utente non è presente nel database, reindirizza alla pagina di autorizzazione
            return RedirectToAction("Authorize", "Login");
        }

        //[HttpGet]
        //public ActionResult EditUserDetails()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserDetails(int id, Users updatedUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Ottieni l'ID dell'utente loggato dal cookie
                    int userID;
                    if (Request.Cookies["IDCookie"] != null && int.TryParse(Request.Cookies["IDCookie"].Value, out userID))
                    {
                        // Trova l'utente nel database utilizzando l'ID
                        var user = db.Users.FirstOrDefault(u => u.ID_Utente == userID);

                        if (user != null)
                        {
                            // Aggiorna le proprietà dell'utente con i valori forniti dall'utente
                            user.Nome = updatedUser.Nome;
                            user.Cognome = updatedUser.Cognome;
                            user.Username = updatedUser.Username;
                            user.Email = updatedUser.Email;
                            user.CodFisc = updatedUser.CodFisc;
                            user.Citta = updatedUser.Citta;
                            user.Indirizzo = updatedUser.Indirizzo;
                            user.Cap = updatedUser.Cap;
                            user.Password = updatedUser.Password;

                            // Salva le modifiche nel database
                            try
                            {
                                db.SaveChanges();
                                return RedirectToAction("GetUserDetails");
                            }
                            catch (DbEntityValidationException ex)
                            {
                                // Se si verifica un errore durante il salvataggio delle modifiche, visualizza un messaggio di errore con i dettagli delle eccezioni di validazione
                                var errorMessages = ex.EntityValidationErrors
                                    .SelectMany(x => x.ValidationErrors)
                                    .Select(x => x.ErrorMessage);
                                var fullErrorMessage = string.Join("; ", errorMessages);
                                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                                return Content(exceptionMessage);
                            }
                            catch (DbUpdateException ex)
                            {
                                // Gestione specifica degli errori di aggiornamento del database
                                return Content("Si è verificato un errore durante l'aggiornamento del database: " + ex.Message);
                            }
                            catch (Exception ex)
                            {
                                // Gestione generica delle altre eccezioni
                                return Content("Si è verificato un errore: " + ex.Message);
                            }
                        }
                        else
                        {
                            return Content("Utente non trovato nel database.");
                        }
                    }
                    else
                    {
                        return Content("ID utente non valido o mancante nel cookie.");
                    }
                }
                else
                {
                    return Content("Il modello non è valido. Assicurati di inserire dati validi.");
                }
            }
            catch (Exception ex)
            {
                // Gestione generica delle eccezioni globali
                return Content("Si è verificato un errore generale: " + ex.Message);
            }
        }





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
