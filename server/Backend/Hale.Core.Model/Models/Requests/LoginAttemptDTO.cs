namespace Hale.Core.Models.Messages
{
    /// <summary>
    /// TODO: Add text here
    /// </summary>
    public class LoginAttemptDTO
    {
        /// <summary>
        /// Username provided from a frontend login attempt
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password in plain text provided from a frontend login attempt
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Persistancy toggle provided from a frontend login attempt
        /// </summary>
        public bool Persistent { get; set; }
    }
}
