using Microsoft.EntityFrameworkCore;

namespace ContactList.Models
{
    /// <summary>
    ///     Kontekt bazy danych.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        ///     Konstruktor.
        /// </summary>
        /// <param name="options">
        ///     Opcje.
        /// </param>
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        ///     U¿ytkownicy.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        ///     Kontakty.
        /// </summary>
        public DbSet<Contact> Contacts { get; set; }

        /// <summary>
        ///     Typy kontaktów.
        /// </summary>
        public DbSet<ContactType> ContactTypes { get; set; }
    }
}