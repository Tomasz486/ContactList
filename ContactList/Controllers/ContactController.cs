using System;
using System.Linq;
using ContactList.Models;
using ContactList.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactList.Controllers
{
    /// <summary>
    ///     Klasa do zarządzania kontaktami.
    /// </summary>
    public class ContactController : Controller
    {
        /// <summary>
        ///     Kontekst bazy danych.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        ///     Konstruktor.
        /// </summary>
        /// <param name="context">
        ///     Kontekst bazy danych.
        /// </param>
        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Główna akcja tego kontrolera - zwraca listę kontaktów.
        /// </summary>
        /// <returns>
        ///     Lista kontaktów.
        /// </returns>
        public IActionResult Index()
        {
            try
            {
                var contacts = _context.Contacts.ToList();
                return Json(contacts);
            }
            catch(Exception)
            {
                return Content("Nie można pobrać listy kontaktów");
            }
        }

        /// <summary>
        ///     Dodawanie kontaktu.
        /// </summary>
        /// <param name="contact">
        ///     Kontakt do zapisania.
        /// </param>
        /// <returns>
        ///     Komunikat o wyniku akcji..
        /// </returns>
        [HttpPost]
        public ActionResult Add([FromBody] ContactWithAuth contact)
        {
            if (!AuthProvider.CheckToken(contact.Token, _context))
            {
                return Content("Tylko zalogowany użytkownik może wykonać tę akcję");
            }

            try
            {
                var contactTypeDb = _context.ContactTypes.FirstOrDefault(c => c.Type == contact.Type);

                if (contactTypeDb == null)
                {
                    _context.ContactTypes.Add(new ContactType(contact.Type));
                }

                _context.Contacts.Add(contact);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return Content("Nie można dodać kontaktu");
            }

            return Content("OK");
        }

        /// <summary>
        ///     Akcja do edycji kontaktu.
        /// </summary>
        /// <param name="contact">
        ///     Kontakt do zapisania.
        /// </param>
        /// <returns>
        ///     Komunikat o wyniku akcji.
        /// </returns>
        [HttpPost]
        public ActionResult Edit([FromBody] ContactWithAuth contact)
        {
            if (!AuthProvider.CheckToken(contact.Token, _context))
            {
                return Content("Tylko zalogowany użytkownik może wykonać tę akcję");
            }

            try
            {
                var dbContact = _context.Contacts.FirstOrDefault(c => c.Id == contact.Id);
                _context.Entry(dbContact).CurrentValues.SetValues(contact);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return Content("Nie można zapisać zmian");
            }

            return Content("OK");
        }

        /// <summary>
        ///     Akcja do pobierania szczegółów wybranego kontaktu z bazy.
        /// </summary>
        /// <param name="contact">
        ///     Dane kontaktu.
        /// </param>
        /// <returns>
        ///     Pełne dane kontaktu lub komunikat o błędzie.
        /// </returns>
        [HttpPost]
        public IActionResult Select([FromBody] ContactWithAuth contact)
        {
            if (!AuthProvider.CheckToken(contact.Token, _context))
            {
                return Content("Tylko zalogowany użytkownik może wykonać tę akcję");
            }

            return Json(_context.Contacts.FirstOrDefault(c => c.Id == contact.Id));
        }

        /// <summary>
        ///     Akcja do usuwania kontaktu.
        /// </summary>
        /// <param name="contact">
        ///     Kontakt do usunięcia.
        /// </param>
        /// <returns>
        ///     Komunikat o wyniku akcji.
        /// </returns>
        [HttpPost]
        public IActionResult Delete([FromBody] ContactWithAuth contact)
        {
            if (!AuthProvider.CheckToken(contact.Token, _context))
            {
                return Content("Tylko zalogowany użytkownik może wykonać tę akcję");
            }

            try
            {
                var dbContact = _context.Contacts.FirstOrDefault(c => c.Id == contact.Id);
                _context.Contacts.Remove(dbContact);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return Content("Nie można usunąc kontaktu");
            }

            return Content("Pomyślnie usunięto kontakt");
        }

        /// <summary>
        ///     Akcja zwraca typy kontaktów.
        /// </summary>
        /// <returns>
        ///     Typy kontaktów.
        /// </returns>
        [HttpPost]
        public IActionResult ContactTypes()
        {
            try
            {
                return Json(_context.ContactTypes.Select(c => c.Type));
            }
            catch (Exception)
            {
                return Content("Nie można pobrać rodzajów kontaktów");
            }
        }
    }
}
