using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace Ctf
{
    // Issue: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-92
    // Reference: http://csharpindepth.com/articles/general/singleton.aspx
    /// <summary>
    /// 
    /// </summary>
    public sealed class ApplicationSettings
    {
        static readonly ApplicationSettings instance = new ApplicationSettings();
        IsolatedStorageSettings settings;
        private static readonly string userKeyword = "user";
        public event EventHandler<EventArgs> UserChanged;

        /// <summary>
        /// Raises the <see cref="E:UserChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnUserChanged(EventArgs e)
        {
            var UserChangedThreadPrivate = UserChanged;
            if (UserChangedThreadPrivate != null)
                UserChangedThreadPrivate(this, e);
        }

        /// <summary>
        /// Initializes the <see cref="ApplicationSettings"/> class.
        /// </summary>
        static ApplicationSettings()
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="ApplicationSettings"/> class from being created.
        /// </summary>
        ApplicationSettings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static ApplicationSettings Instance
        {
            get { return instance; }
        }

        // Reference : http://www.geekchamp.com/tips/all-about-wp7-isolated-storage-store-data-in-isolatedstoragesettings
        /// <summary>
        /// Saves the logged user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public bool SaveLoggedUser(User user)
        {
            if ((user != null) && (!user.HasNullOrEmpty()) && (!settings.Contains(userKeyword)))
            {
                Debug.WriteLine("SaveLoggedUser added User");
                settings.Add(userKeyword, user);
                // TDOD Localize string
                OnUserChanged(new MessengerSentEventArgs("User " + user.username + " has been saved.", ErrorCode.SUCCESS));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrives the logged user.
        /// </summary>
        /// <returns></returns>
        public User RetriveLoggedUser()
        {
            Debug.WriteLine("public User RetriveLoggedUser()");
            Debug.WriteLineIf(!settings.Contains(userKeyword), "ApplicationSettings does NOT contain User.");
            Debug.WriteLineIf(settings.Contains(userKeyword), "ApplicationSettings CONTAINS User.");

            User user = new User();
            if (settings.Contains(userKeyword))
            {
                settings.TryGetValue<User>(userKeyword, out user);
            }
            return user;
        }

        /// <summary>
        /// Removes from settings.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <returns></returns>
        public bool RemoveFromSettings(string keyword)
        {
            if (settings.Contains(keyword))
            {
                Debug.WriteLine("Removed from settings: " + keyword);
                settings.Remove(keyword);
                // TDOD Localize string
                OnUserChanged(new MessengerSentEventArgs("User has been Removed", ErrorCode.SUCCESS));
                return true;
            }
            return false;
        }
    }
}
