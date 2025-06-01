using System;
using System.Linq;
using ContactList.Models;
using ContactList.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactList.Controllers
{
    /// <summary>
    ///     Klasa do obsługi konta użytkownika.
    /// </summary>
    /// <param name="context"></param>
    public class UserController(AppDbContext context) : Controller
    {
        /// <summary>
        ///     Kontekt bazy danych.
        /// </summary>
        private readonly AppDbContext _context = context;

        /// <summary>
        ///     Akcja tworzenia konta użytkownika.
        /// </summary>
        /// <param name="userData">
        ///     Dane użytkonika.
        /// </param>
        /// <returns>
        ///     Komunikat o wyniku akcji.
        /// </returns>
        [HttpPost]
        public ActionResult Create([FromBody] UserData userData)
        {
            if (userData == null)
            {
                return Content("Nie można utworzyć nowego użytkownika");
            }

            var user = new User
            {
                Login = userData.Login,
                Salt = AuthProvider.CreateSalt(10)
            };
            user.Password = AuthProvider.Hash(userData.Password, user.Salt);
            _context.Users.Add(user);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return Content("Nie można utworzyć nowego użytkownika. Upewnij się, czy taki login już istnieje");
            }

            return Content("OK");
        }

        /// <summary>
        ///     Akcja do logowania użytkownika.
        /// </summary>
        /// <param name="userData">
        ///     Dane użytkownika.
        /// </param>
        /// <returns>
        ///     Komunikat o wyniku akcji.
        /// </returns>
        [HttpPost]
        public ActionResult LogIn([FromBody] UserData userData)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Login == userData.Login);

                if (user != null && AuthProvider.ConfirmPassword(userData.Password, user.Password, user.Salt))
                {
                    userData.Token = "Token";
                    user.Token = userData.Token;
                    user.LastLogin = DateTime.Now;

                    try
                    {
                        _context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        return Content("Problem z utworzeniem tokenu");
                    }

                    return Json(userData);
                }
                else
                {
                    return Content("Błędny login lub hasło");
                }
            }
            catch (Exception)
            {
                return Content("Problem z logowaniem");
            }
        }

        /// <summary>
        ///     Akcja do wylogowania użytkownika.
        /// </summary>
        /// <param name="token">
        ///     Token.
        /// </param>
        /// <returns>
        ///     Komunikat o wyniku akcji.
        /// </returns>

        [HttpPost]
        public ActionResult LogOut([FromBody] string token)
        {
            try
            {
                var user = _context.Users.First(u => u.Token == token);
                user.Token = null;
            }
            catch (Exception)
            {
                return Content("Problem z wylogowaniem użytkownika");
            }

            return Content("OK");
        }

        [HttpPost]
        public ActionResult CheckToken([FromBody] string token)
        {
            if (!AuthProvider.CheckToken(token, _context))
            {
                return Content("Tylko zalogowany użytkownik może wykonać tę akcję");
            }

            return Content("OK");
        }
    }
}
