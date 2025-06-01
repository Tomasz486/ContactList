using Microsoft.AspNetCore.Mvc;

namespace ContactList.Controllers
{
    /// <summary>
    ///     Kontroler strony g��wnwej.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        ///     Strona g��wna.
        /// </summary>
        /// <returns>
        ///     Widok strony g��wnej.
        /// </returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}