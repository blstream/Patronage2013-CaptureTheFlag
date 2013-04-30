using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;


namespace Ctf
{
    // reference : http://mobile.dzone.com/news/windows-phone-7-serialization-0
    /// <summary>
    /// 
    /// </summary>
    [DataContractAttribute]
    public class User
    {
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string scope { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            username = String.Empty;
            access_token = String.Empty;
            token_type = String.Empty;
            scope = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="access_token">The access_token.</param>
        /// <param name="token_type">The token_type.</param>
        /// <param name="scope">The scope.</param>
        public User(string username, string access_token, string token_type, string scope)
        {
            if (username != null)
                this.username = username;
            else
                this.username = String.Empty;

            if (access_token != null)
                this.access_token = access_token;
            else
                this.access_token = String.Empty;

            if (token_type != null)
                this.token_type = token_type;
            else
                this.token_type = String.Empty;

            if (scope != null)
                this.scope = scope;
            else
                this.scope = String.Empty;
        }

        /// <summary>
        /// Determines whether [has null or empty].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has null or empty]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasNullOrEmpty()
        {
            return (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(access_token) || String.IsNullOrEmpty(token_type) || String.IsNullOrEmpty(scope));
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string userJSON = "{ ";
            userJSON += "\"username\" : \"" + username + "\", ";
            userJSON += "\"access_token\" : \"" + access_token + "\", ";
            userJSON += "\"token_type\" : \"" + token_type + "\", ";
            userJSON += "\"scope\" : \"" + scope + "\" ";
            userJSON += " }";
            return userJSON;
        }
    }
}
