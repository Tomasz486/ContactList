using Microsoft.AspNetCore.Mvc;

namespace ContactList.Controllers
{
    /// <summary>
    ///     Kontroler strony g³ównwej.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        ///     Strona g³ówna.
        /// </summary>
        /// <returns>
        ///     Widok strony g³ównej.
        /// </returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}