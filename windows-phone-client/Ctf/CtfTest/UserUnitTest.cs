using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Ctf;

namespace CtfTest
{
    [TestClass]
    public class UserUnitTest
    {
        [TestMethod]
        public void TestUserConstructor()
        {
            string username = "piotrekm44@o2.pl";
            string access_token = "d53e57c6-cb45-481c-9ed4-df15803f0e0d";
            string token_type = "bearer";
            string scope = "read write";
            User emptyUser = new User();
            User allNullUser = new User(null, null, null, null);
            User allFilledUser = new User(username, access_token, token_type, scope);
            
            Assert.AreEqual(emptyUser.username, String.Empty, "Username should be empty.");
            Assert.AreEqual(emptyUser.access_token, String.Empty, "Access token should be empty.");
            Assert.AreEqual(emptyUser.token_type, String.Empty, "Token type should be empty.");
            Assert.AreEqual(emptyUser.scope, String.Empty, "Scope should be empty.");

            Assert.AreEqual(allNullUser.username, String.Empty, "Username should be empty.");
            Assert.AreEqual(allNullUser.access_token, String.Empty, "Access token should be empty.");
            Assert.AreEqual(allNullUser.token_type, String.Empty, "Token type should be empty.");
            Assert.AreEqual(allNullUser.scope, String.Empty, "Scope should be empty.");

            Assert.AreEqual(allFilledUser.username, username, "Username should be empty.");
            Assert.AreEqual(allFilledUser.access_token, access_token, "Access token should be empty.");
            Assert.AreEqual(allFilledUser.token_type, token_type, "Token type should be empty.");
            Assert.AreEqual(allFilledUser.scope, scope, "Scope should be empty.");
        }

        [TestMethod]
        public void TestHasNullOrEmpty()
        {
            string username = "piotrekm44@o2.pl";
            string access_token = "d53e57c6-cb45-481c-9ed4-df15803f0e0d";
            string token_type = "bearer";
            string scope = "read write";
            User allEmptyUser = new User(null, null, null, null);
            User firstMixedEmptyUser = new User(username, null, token_type, null);
            User secondMixedEmptyUser = new User(null, access_token, null, scope);
            User allFilledUser = new User(username, access_token, token_type, scope);

            Assert.IsTrue(allEmptyUser.HasNullOrEmpty(), "Should have a field that is empty.");
            Assert.IsTrue(firstMixedEmptyUser.HasNullOrEmpty(), "Should have a field that is empty.");
            Assert.IsTrue(secondMixedEmptyUser.HasNullOrEmpty(), "Should have a field that is empty.");
            Assert.IsFalse(allFilledUser.HasNullOrEmpty(), "Should NOT have a field that is empty.");
        }

        [TestMethod]
        public void TestToString()
        {
            string username = "piotrekm44@o2.pl";
            string access_token = "d53e57c6-cb45-481c-9ed4-df15803f0e0d";
            string token_type = "bearer";
            string scope = "read write";
            string FullUserJSON = "{ ";
            FullUserJSON += "\"username\" : \"" + username + "\", ";
            FullUserJSON += "\"access_token\" : \"" + access_token + "\", ";
            FullUserJSON += "\"token_type\" : \"" + token_type + "\", ";
            FullUserJSON += "\"scope\" : \"" + scope + "\" ";
            FullUserJSON += " }";
            string EmptyUserJSON = "{ ";
            EmptyUserJSON += "\"username\" : \"" + String.Empty + "\", ";
            EmptyUserJSON += "\"access_token\" : \"" + String.Empty + "\", ";
            EmptyUserJSON += "\"token_type\" : \"" + String.Empty + "\", ";
            EmptyUserJSON += "\"scope\" : \"" + String.Empty + "\" ";
            EmptyUserJSON += " }";
            User allFilledUser = new User(username, access_token, token_type, scope);
            User allEmptyUser = new User(String.Empty, String.Empty, String.Empty, String.Empty);

            Assert.AreEqual(allFilledUser.ToString(), FullUserJSON);
            Assert.AreEqual(allEmptyUser.ToString(), EmptyUserJSON);
        }
    }
}
