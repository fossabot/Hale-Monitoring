using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hale.Core.Models.Shared
{
    /// <summary>
    /// Message for returning login responses to the GUI.
    /// </summary>
    public class LoginResponse
    {
        public enum LoginResponseType
        {
            Success,
            InvalidCredentials,
            OldPassword,
            InternalError,
            Unknown
        }

        public LoginResponseType ResponseType { get; set; } = LoginResponseType.Unknown;

        /// <summary>
        /// Returns the user id for the authenticated user.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Returns any errors during the authentication process.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Returns the date the password was changed if the old password was supplied
        /// </summary>
        public DateTime PasswordChanged { get; set; }

        /// <summary>
        /// Construct a error response
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static LoginResponse ErrorResponse(string error)
        {
            return new LoginResponse() {
                UserId = -1,
                Error = error,
                ResponseType = LoginResponseType.InternalError
            };
        }

        /// <summary>
        /// Construct a invalid credentials response
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static LoginResponse InvalidCredentialsResponse()
        {
            return new LoginResponse()
            {
                UserId = -1,
                Error = "Invalid credentials",
                ResponseType = LoginResponseType.InvalidCredentials
            };
        }

        public static LoginResponse PasswordChangedResponse(DateTime changedTime)
        {
            return new LoginResponse()
            {
                UserId = -1,
                Error = "Password was changed",
                PasswordChanged = changedTime,
                ResponseType = LoginResponseType.OldPassword
            };
        }

    }
}
