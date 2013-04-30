using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ctf
{
    /// <summary>
    /// 
    /// </summary>
    public class UserCredentials
    {
        public static int MINIMAL_USERNAME_LENGTH = 5;
        public static int MINIMAL_PASSWORD_LENGTH = 5;
        private string username;
        private string password;

        public static event EventHandler<EventArgs> MessengerSent;

        /// <summary>
        /// Raises the <see cref="E:MessengerSent" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnMessengerSent(EventArgs e)
        {
            var MessengerSentThreadPrivate = MessengerSent;
            if (MessengerSentThreadPrivate != null)
                MessengerSentThreadPrivate(this, e);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCredentials"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public UserCredentials(string username, string password)
        {
            if (!IsUsernameProperLength(username))
            {
                this.username = String.Empty;
                OnMessengerSent(new MessengerSentEventArgs("Username lenght should be at least " + MINIMAL_USERNAME_LENGTH + " characters long."));
            }
            else
            {
                this.username = username;
            }
            if (!IsPasswordProperLength(password))
            {
                this.password = String.Empty;
                OnMessengerSent(new MessengerSentEventArgs("Password lenght should be at least " + MINIMAL_PASSWORD_LENGTH + " characters long."));
            }
            else
            {
                this.password = password;
            }
        }

        /// <summary>
        /// Determines whether [is username proper length] [the specified username].
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        ///   <c>true</c> if [is username proper length] [the specified username]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUsernameProperLength(string username)
        {
            if(username == null)
                return false;
            return (username.Length >= MINIMAL_USERNAME_LENGTH);
        }

        /// <summary>
        /// Determines whether [is password proper length] [the specified password].
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c> if [is password proper length] [the specified password]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPasswordProperLength(string password)
        {
            if(password == null)
                return false;
            return (password.Length >= MINIMAL_PASSWORD_LENGTH);
        }

        /// <summary>
        /// Determines whether [has matching password] [the specified password].
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c> if [has matching password] [the specified password]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasMatchingPassword(string password)
        {
            if (password == null)
                return false;
            return this.password.Equals(password);
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <returns></returns>
        public string GetUsername()
        {
            return this.username;
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <returns></returns>
        public string GetPassword()
        {
            return this.password;
        }
    }
}
