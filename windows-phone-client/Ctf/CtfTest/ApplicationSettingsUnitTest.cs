using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Ctf;

namespace CtfTest
{
    [TestClass]
    public class ApplicationSettingsUnitTest
    {
        [TestMethod]
        public void TestSaveLoggedUser()
        {
            string userKeyword = "user";
            User nullUser = null;
            User defaultUser = new User();
            User properUser = new User("piotrekm44@o2.pl", "d53e57c6-cb45-481c-9ed4-df15803f0e0d", "bearer", "read write");
            bool nullResult;
            bool defaultResult;
            bool firstProperResult;
            bool secondProperResult;

            ApplicationSettings.Instance.RemoveFromSettings(userKeyword);
            nullResult = ApplicationSettings.Instance.SaveLoggedUser(nullUser);
            defaultResult = ApplicationSettings.Instance.SaveLoggedUser(defaultUser);
            firstProperResult = ApplicationSettings.Instance.SaveLoggedUser(properUser);
            secondProperResult = ApplicationSettings.Instance.SaveLoggedUser(properUser);
            ApplicationSettings.Instance.RemoveFromSettings(userKeyword);

            Assert.IsFalse(nullResult, "Null is saved.");
            Assert.IsFalse(defaultResult, "Empty user is saved.");
            Assert.IsTrue(firstProperResult, "User is not saved.");
            Assert.IsFalse(secondProperResult, "User is overwritten.");
        }

        [TestMethod]
        public void TestRetriveLoggedUser()
        {
            string userKeyword = "user";
            User emptyResultUser = null;
            User properResultUser = null;
            User properUser = new User("piotrekm44@o2.pl", "d53e57c6-cb45-481c-9ed4-df15803f0e0d", "bearer", "read write");
            bool properSavedResult;

            ApplicationSettings.Instance.RemoveFromSettings(userKeyword);
            emptyResultUser = ApplicationSettings.Instance.RetriveLoggedUser();
            properSavedResult = ApplicationSettings.Instance.SaveLoggedUser(properUser);
            properResultUser = ApplicationSettings.Instance.RetriveLoggedUser();
            ApplicationSettings.Instance.RemoveFromSettings(userKeyword);

            Assert.IsTrue(emptyResultUser.HasNullOrEmpty(), "User should be empty.");
            Assert.IsTrue(properSavedResult, "User is not saved.");
            Assert.IsFalse(properResultUser.HasNullOrEmpty(), "User should NOT be empty.");
        }

        [TestMethod]
        public void TestRemoveFromSettings()
        {
            string userKeyword = "user";
            bool userNotRemoved;
            User properUser = new User("piotrekm44@o2.pl", "d53e57c6-cb45-481c-9ed4-df15803f0e0d", "bearer", "read write");
            bool properSavedResult;
            bool userRemoved;

            ApplicationSettings.Instance.RemoveFromSettings(userKeyword);
            userNotRemoved = ApplicationSettings.Instance.RemoveFromSettings(userKeyword);
            properSavedResult = ApplicationSettings.Instance.SaveLoggedUser(properUser);
            userRemoved = ApplicationSettings.Instance.RemoveFromSettings(userKeyword);
            ApplicationSettings.Instance.RemoveFromSettings(userKeyword);

            Assert.IsFalse(userNotRemoved, "No user inside but removal occured.");
            Assert.IsTrue(properSavedResult, "User is not saved.");
            Assert.IsTrue(userRemoved, "User was not removed.");
        }

        [TestMethod]
        public void TestOnUserChanged()
        {
            string userKeyword = "user";
            ApplicationSettings.Instance.RemoveFromSettings(userKeyword);
            ApplicationSettings.Instance.UserChanged += HelperOnUserChanged;
            User properUser = new User("piotrekm44@o2.pl", "d53e57c6-cb45-481c-9ed4-df15803f0e0d", "bearer", "read write");
            ApplicationSettings.Instance.RemoveFromSettings(userKeyword);
            ApplicationSettings.Instance.UserChanged -= HelperOnUserChanged;
        }

        public void HelperOnUserChanged(object sender, EventArgs e)
        {
            Assert.IsInstanceOfType(e, typeof(MessengerSentEventArgs), "EventArgs is not type of MessengerSentEventArgs");
            string message = ((MessengerSentEventArgs)e).message;
            Assert.IsNotNull(message, "Message should not be null.");
        }
    }
}
