using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Ctf;

namespace CtfTest
{
    [TestClass]
    public class UserCredentialsUnitTest
    {
        [TestMethod]
        public void TestIsUsernameProperLength()
        {
            string properLengthInput = new string('a', UserCredentials.MINIMAL_USERNAME_LENGTH);
            string improperLenghtInput = "a";
            bool properLenghtResult;
            bool improperLenghtResult;

            properLenghtResult = UserCredentials.IsUsernameProperLength(properLengthInput);
            improperLenghtResult = UserCredentials.IsUsernameProperLength(improperLenghtInput);

            Assert.IsTrue(properLenghtResult, "Length test should pass.");
            Assert.IsFalse(improperLenghtResult, "Length test should NOT pass.");
        }

        [TestMethod]
        public void TestIsPasswordProperLength()
        {
            string properLengthInput = new string('a', UserCredentials.MINIMAL_PASSWORD_LENGTH);
            string improperLenghtInput = "a";
            bool properLenghtResult;
            bool improperLenghtResult;

            properLenghtResult = UserCredentials.IsPasswordProperLength(properLengthInput);
            improperLenghtResult = UserCredentials.IsPasswordProperLength(improperLenghtInput);

            Assert.IsTrue(properLenghtResult, "Length test should pass.");
            Assert.IsFalse(improperLenghtResult, "Length test should NOT pass.");
        }

        [TestMethod]
        public void TestHasMatchingPassword()
        {
            string properLengthInput = new string('a', UserCredentials.MINIMAL_PASSWORD_LENGTH);
            string matchingPassword = new string('a', UserCredentials.MINIMAL_PASSWORD_LENGTH);
            string notMatchingPassword = new string('b', UserCredentials.MINIMAL_PASSWORD_LENGTH);
            UserCredentials userCredentials = new UserCredentials(properLengthInput, properLengthInput);
            bool matchingPasswordResult;
            bool notMatchingPasswordResult;

            matchingPasswordResult = userCredentials.HasMatchingPassword(matchingPassword);
            notMatchingPasswordResult = userCredentials.HasMatchingPassword(notMatchingPassword);

            Assert.IsTrue(matchingPasswordResult, "Should be a match.");
            Assert.IsFalse(notMatchingPasswordResult, "Should not be a match.");
        }

        [TestMethod]
        public void TestUserCredentialsConstructor()
        {
            string properLengthInput = new string('a', UserCredentials.MINIMAL_PASSWORD_LENGTH);
            string improperLenghtInput = "a";
            int expectedMessageCount = 4;
            UserCredentials.MessengerSent += HelperOnMessangerSent;
            UserCredentials userCredentialsImpImp = new UserCredentials(improperLenghtInput, improperLenghtInput);
            UserCredentials userCredentialsImpProp = new UserCredentials(improperLenghtInput, properLengthInput);
            UserCredentials userCredentialsPropImp = new UserCredentials(properLengthInput, improperLenghtInput);
            UserCredentials userCredentialsPropProp = new UserCredentials(properLengthInput, properLengthInput);
            UserCredentials.MessengerSent -= HelperOnMessangerSent;

            Assert.AreEqual(userCredentialsImpImp.GetUsername(), String.Empty);
            Assert.AreEqual(userCredentialsImpImp.GetPassword(), String.Empty);
            Assert.AreEqual(userCredentialsImpProp.GetUsername(), String.Empty);
            Assert.AreEqual(userCredentialsImpProp.GetPassword(), properLengthInput);
            Assert.AreEqual(userCredentialsPropImp.GetUsername(), properLengthInput);
            Assert.AreEqual(userCredentialsPropImp.GetPassword(), String.Empty);
            Assert.AreEqual(userCredentialsPropProp.GetUsername(), properLengthInput);
            Assert.AreEqual(userCredentialsPropProp.GetPassword(), properLengthInput);
            Assert.AreEqual(expectedMessageCount, messageCount, "Message count is not expected.");
        }

        private static int messageCount = 0;

        public void HelperOnMessangerSent(object sender, EventArgs e)
        {
            Assert.IsInstanceOfType(e, typeof(MessengerSentEventArgs), "EventArgs is not type of MessengerSentEventArgs");
            string message = ((MessengerSentEventArgs)e).message;
            Assert.IsNotNull(message, "Message should not be null.");
            messageCount++;
        }
    }
}
