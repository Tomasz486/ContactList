using System.ComponentModel.DataAnnotations;
using System;

namespace ContactList.Models
{
    /// <summary>
    ///     U¿ytkownik.
    /// </summary>
    public class User
    {
        /// <summary>
        ///     Id.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        ///     Login.
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        ///     Has³o.
        /// </summary>
        [Required]
        public byte[] Password { get; set; }

        /// <summary>
        ///     Data ostatniego logowania.
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]

        public DateTime LastLogin { get; set; }

        /// <summary>
        ///     Token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///     Sól do has³a.
        /// </summary>
        public byte[] Salt { get; set; }
    }
}