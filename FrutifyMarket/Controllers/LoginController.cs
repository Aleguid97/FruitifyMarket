using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FrutifyMarket.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Authorize()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Username e Password sono obbligatori.");
                return View();
            }

            using (var context = new ModelDBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(username, false);

                    HttpCookie LoginCookie = new HttpCookie("IDCookie");
                    LoginCookie.Value = user.ID_Utente.ToString();
                    LoginCookie.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(LoginCookie);


                    if (!string.IsNullOrEmpty(user.Nome))
                    {
                        TempData["LoginMessage"] = "Benvenuto " + user.Nome + " " + user.Cognome;
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username o Password errati.");
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            TempData["LogoutMessage"] = "Sei stato disconnesso correttamente.";
            return RedirectToAction("Index", "Home");
        }
    }
}
