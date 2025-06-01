namespace ContactList.Models
{
    /// <summary>
    ///     Typ kontaktu.
    /// </summary>
    public class ContactType
    {
        /// <summary>
        ///     Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Konstruktor.
        /// </summary>
        /// <param name="type">
        ///     Rodzaj kontaktu.
        /// </param>
        public ContactType(string type)
        {
            this.Type = type;
        }

        /// <summary>
        ///     Rodzaj kontaktu.
        /// </summary>
        public string Type { get; set; }
    }
}
