namespace ContactList.Models
{
    /// <summary>
    ///     Klasa dziedzicząca po Contact celem dodania autoryzacji dla akcji.
    /// </summary>
    public class ContactWithAuth : Contact
    {
        /// <summary>
        ///     Token.
        /// </summary>
        public string Token { get; set; }
    }
}
