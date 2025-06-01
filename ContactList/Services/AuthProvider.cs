using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ContactList.Models;

namespace ContactList.Services
{
    /// <summary>
    ///     Klasa do generowania i porównywania Hashy z haseł.
    /// </summary>
    public class AuthProvider
    {
        /// <summary>
        ///     Czas w minutach, po którym token traci ważność.
        /// </summary>
        const int LoginTimeOut = 3;

        /// <summary>
        ///     Metoda generująca sól do hashowania hasła.
        /// </summary>
        /// <param name="size">
        ///     Rozmiar soli.
        /// </param>
        /// <returns>
        ///     Sól do hashowania.
        /// </returns>
        public static byte[] CreateSalt(int size)
        {
            return RandomNumberGenerator.GetBytes(size);
        }

        /// <summary>
        ///     Metoda generująca hash z podanego tekstu.
        /// </summary>
        /// <param name="value">
        ///     Tekst to zhashowania.
        /// </param>
        /// <param name="salt">
        ///     Sól do hashowania.
        /// </param>
        /// <returns>
        ///     Hash tekstu.
        /// </returns>
        public static byte[] Hash(string value, byte[] salt)
        {
            return Hash(Encoding.UTF8.GetBytes(value), salt);
        }

        /// <summary>
        ///     Metoda generująca hash z podanej tablicy bajtów.
        /// </summary>
        /// <param name="value">
        ///     Tablica bajtów do zhashowania.
        /// </param>
        /// <param name="salt">
        ///     Sól do hashowania.
        /// </param>
        /// <returns>
        ///     Hash tablicy bajtów.
        /// </returns>
        public static byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();

            return new XSystem.Security.Cryptography.SHA256Managed().ComputeHash(saltedValue);
        }

        /// <summary>
        ///     Metoda porównująca hasła.
        /// </summary>
        /// <param name="password">
        ///     Hasło do porównania.
        /// </param>
        /// <param name="passwordHash2">
        ///     Drugie zhashowane hasło do porównania.
        /// </param>
        /// <param name="salt">
        ///     Sól do hashowania.
        /// </param>
        /// <returns>
        ///     Zwraca true jeśli hasła są identyczne (funkcja hashująca zwraca dla obu ten sam wynik)
        /// </returns>
        public static bool ConfirmPassword(string password, byte[] passwordHash2, byte[] salt)
        {
            byte[] passwordHash = Hash(password, salt);

            return passwordHash.SequenceEqual(passwordHash2);
        }

        /// <summary>
        ///     Sprawdza, czy token jest prawidłowy.
        /// </summary>
        /// <param name="token">
        ///     Token.
        /// </param>
        /// <param name="context">
        ///     Kontekst bazy danych.
        /// </param>
        /// <returns>
        ///     Zwraca true jeśli token jest prawidłowy.
        /// </returns>
        public static bool CheckToken(string token, AppDbContext context)
        {
            var user = context.Users.FirstOrDefault(u => u.Token == token);

            if (user == null || DateTime.Now.Subtract(user.LastLogin) > TimeSpan.FromMinutes(LoginTimeOut))
            {
                return false;
            }


            return true;
        }

    }
}
