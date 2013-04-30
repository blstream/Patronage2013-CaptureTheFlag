using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ctf
{
    // User story: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-56
    /// <summary>
    /// 
    /// </summary>
    class Login
    {
        private RequestHandler requestHandler;
        private RestRequest request;
        private string username;

        public event EventHandler<EventArgs> MessengerSent;

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
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login()
        {
            username = String.Empty;

            requestHandler = new RequestHandler();

            request = new RestRequest("/oauth/token", Method.POST);
            request.AddHeader("Content-type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", "mobile_windows");
            request.AddParameter("grant_type", "password");
        }

        /// <summary>
        /// Requests the callback on success.
        /// </summary>
        /// <param name="response">The response.</param>
        public void RequestCallbackOnSuccess(IRestResponse<LoginJsonResponse> response)
        {
            if ((response != null) && (response.Data != null))
            {
                if (String.IsNullOrEmpty(response.Data.error))
                {
                    if (String.IsNullOrEmpty(response.Data.access_token) && String.IsNullOrEmpty(response.Data.token_type) && String.IsNullOrEmpty(response.Data.scope))
                    {
                        Debug.WriteLine("Response Data is NULL or EMPTY.");
                        Debug.WriteLine("MessangerSent: " + "Unknown error, please try again later.");
                        OnMessengerSent(new MessengerSentEventArgs("Unknown error, please try again later."));
                    }
                    else
                    {
                        Debug.WriteLine("Response Data retrieved SUCCESSFULY.");
                        Debug.WriteLine("response.Data.access_token: " + response.Data.access_token);
                        Debug.WriteLine("response.Data.token_type: " + response.Data.token_type);
                        Debug.WriteLine("response.Data.scope: " + response.Data.scope);
                        ApplicationSettings.Instance.SaveLoggedUser(new User(username, response.Data.access_token, response.Data.token_type, response.Data.scope));
                    }
                }
                else
                {
                    Debug.WriteLine("Response Data is an ERROR.");
                    Debug.WriteLine("MessangerSent: " + response.Data.error + ": " + response.Data.error_description);
                    OnMessengerSent(new MessengerSentEventArgs(response.Data.error + ": " + response.Data.error_description));
                }
            }
        }

        /// <summary>
        /// Requests the callback on fail.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public void RequestCallbackOnFail(String errorMessage)
        {
            Debug.WriteLine("MessangerSent: " + errorMessage);
            OnMessengerSent(new MessengerSentEventArgs(errorMessage));
        }

        // TODO: Check if is async
        // Issue: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-87
        /// <summary>
        /// Logs the in as.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="secret">The secret.</param>
        /// <returns></returns>
        public async Task<RestRequestAsyncHandle> LogInAs(UserCredentials user, string secret)
        {
            Debug.WriteLine("secret could be: " + System.Guid.NewGuid().ToString());
            if (LoggedAs() != null)
            {
                Debug.WriteLine("MessangerSent: " + "Logged in as another user. Please, logout first");
                OnMessengerSent(new MessengerSentEventArgs("Logged in as another user. Please, logout first"));
            }

            if (String.IsNullOrWhiteSpace(secret))
            {
                Debug.WriteLine("MessangerSent: " + "No secret key is set. Please reinstall this app.");
                OnMessengerSent(new MessengerSentEventArgs("No secret key is set. Please reinstall this app."));
            }
            else
                request.AddParameter("client_secret", secret);

            if (user == null)
            {
                Debug.WriteLine("MessangerSent: " + "Lost user credentials. Please login once more");
                OnMessengerSent(new MessengerSentEventArgs("Lost user credentials. Please login once more"));
            }
            else
            {
                request.AddParameter("username", user.GetUsername());
                request.AddParameter("password", user.GetPassword());
                username = user.GetUsername();
            }
            return await requestHandler.ExecuteAsync<LoginJsonResponse>(request, RequestCallbackOnSuccess, RequestCallbackOnFail);
        }

        // Issue: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-89
        /// <summary>
        /// Loggeds as.
        /// </summary>
        /// <returns></returns>
        public User LoggedAs()
        {
            return ApplicationSettings.Instance.RetriveLoggedUser();
        }

        // Issue: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-89
        /// <summary>
        /// Logs the out.
        /// </summary>
        /// <returns></returns>
        public bool LogOut()
        {
            return ApplicationSettings.Instance.RemoveFromSettings("user");
        }
    }
}
