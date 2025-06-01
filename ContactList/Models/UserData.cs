namespace ContactList.Models
{
    /// <summary>
    ///     Klasa do obsługi logowania użytkownika.
    /// </summary>
    public class UserData
    {
        /// <summary>
        ///     Login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        ///     Hasło.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Token.
        /// </summary>
        public string Token { get; set; }
    }
}
