namespace ContactList.Models
{
    /// <summary>
    ///     kontakt.
    /// </summary>
    public class Contact
    {
        /// <summary>
        ///     Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Imiê.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        ///     Nazwisko.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        ///     E-mail.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Telefon.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///     Rodzaj kontaktu.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Dzieñ urodzin.
        /// </summary>
        public string DayOfBirth { get; set; }
    }
}