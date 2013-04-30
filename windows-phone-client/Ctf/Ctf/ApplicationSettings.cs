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
    // TODO incorporate events
    public sealed class ApplicationSettings
    {
        static readonly ApplicationSettings instance = new ApplicationSettings();
        IsolatedStorageSettings settings;
        public event EventHandler<EventArgs> UserChanged;

        private void OnUserChanged(EventArgs e)
        {
            var UserChangedThreadPrivate = UserChanged;
            if (UserChangedThreadPrivate != null)
                UserChangedThreadPrivate(this, e);
        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static ApplicationSettings()
        {
            Debug.WriteLine("static ApplicationSettings() [Constructor]");
        }

        ApplicationSettings()
        {
            Debug.WriteLine("ApplicationSettings() [Constructor]");

            settings = IsolatedStorageSettings.ApplicationSettings;
            Debug.WriteLineIf(settings == null, "ApplicationSettings() : IsolatedStorageSettings settings is NULL");
        }

        public static ApplicationSettings Instance
        {
            get { return instance; }
        }

        // Reference : http://www.geekchamp.com/tips/all-about-wp7-isolated-storage-store-data-in-isolatedstoragesettings
        public void SaveLoggedUser(User user)
        {
            Debug.WriteLineIf(user == null, "SaveLoggedUser's user is NULL");
            if (user != null)
            {
                Debug.WriteLineIf(settings == null, "SaveLoggedUser's settings is NULL");
                Debug.WriteLineIf(!settings.Contains("user"), "SaveLoggedUser's settings does NOT contain user");
                Debug.WriteLineIf(settings.Contains("user"), "SaveLoggedUser's settings CONTAINS user");
                if ((settings != null) && (!settings.Contains("user")))
                {
                    Debug.WriteLine("SaveLoggedUser added user");
                    settings.Add("user", user);
                    OnUserChanged(EventArgs.Empty);
                }
            }
        }

        public User RetriveLoggedUser()
        {
            User user = null;
            Debug.WriteLineIf(settings == null, "RetriveLoggedUser's settings is NULL");
            Debug.WriteLineIf(!settings.Contains("user"), "RetriveLoggedUser's settings does NOT contain user");
            Debug.WriteLineIf(settings.Contains("user"), "RetriveLoggedUser's settings CONTAINS user");
            if ((settings != null) && settings.Contains("user"))
            {
                user = new User(null, null, null, null);
                //TODO: handle exceptions
                settings.TryGetValue<User>("user", out user);

                if (user.HasNullOrEmpty())
                {
                    Debug.WriteLine("RetriveLoggedUser's user.hasNullOrEmpty() == true");
                    user = null;
                }
            }
            Debug.WriteLineIf(user == null, "RetriveLoggedUser's user is NULL");
            return user;
        }

        public bool RemoveFromSettings(string keyword)
        {
            if ((settings != null) && settings.Contains(keyword))
            {
                Debug.WriteLine("Removed from settings: " + keyword);
                settings.Remove(keyword);
                OnUserChanged(EventArgs.Empty);
                return true;
            }
            return false;
        }
    }
}
