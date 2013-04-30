using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Diagnostics;

namespace Ctf
{
    // BUG?: new { username = "a", password = "a" } registers and gets a token but is not visible in 'Player list' at Capture the flag :: API
    // User story: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-55
    /// <summary>
    /// 
    /// </summary>
    class Registration
    {
        private RequestHandler requestHandler;
        private RestRequest request;

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
        /// Initializes a new instance of the <see cref="Registration"/> class.
        /// </summary>
        public Registration()
        {
            requestHandler = new RequestHandler();

            request = new RestRequest("/api/players/add", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-type", "application/json");
            request.RequestFormat = DataFormat.Json;
        }

        //* Nazwa użytkownika musi być unikalna.
        /// <summary>
        /// Requests the callback on success.
        /// </summary>
        /// <param name="response">The response.</param>
        private void RequestCallbackOnSuccess(IRestResponse<ServerJsonResponse> response)
        {
            if ((response != null) && (response.Data != null))
            {
                Debug.WriteLine("Content: " + response.Content);
                if (String.IsNullOrEmpty(response.Data.error))
                {
                    Debug.WriteLine("MessangerSent: " + "Response Data retrieved SUCCESSFULY.");
                    Debug.WriteLine("response.Data.error_code: " + response.Data.error_code);
                    OnMessengerSent(new MessengerSentEventArgs("Registration has been successful.", ErrorCode.SUCCESS));
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
        private void RequestCallbackOnFail(String errorMessage)
        {
            Debug.WriteLine("MessangerSent: " + "Request has failed: " + errorMessage);
            OnMessengerSent(new MessengerSentEventArgs("Request has failed: " + errorMessage));
        }

        // TODO: Check if is async
        // Issue: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-88
        /// <summary>
        /// Registers the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="verifyPassword">The verify password.</param>
        /// <returns></returns>
        public async Task<RestRequestAsyncHandle> Register(UserCredentials user, string verifyPassword)
        {
            if (user != null)
            {
                if (user.HasMatchingPassword(verifyPassword))
                {
                    request.AddBody(new { username = user.GetUsername(), password = user.GetPassword() });
                    return await requestHandler.ExecuteAsync<ServerJsonResponse>(request, RequestCallbackOnSuccess, RequestCallbackOnFail);
                }
                else
                {
                    Debug.WriteLine("MessangerSent: " + "Passwords does not match.");
                    OnMessengerSent(new MessengerSentEventArgs("Passwords does not match."));
                }

            }
            else
            {
                Debug.WriteLine("MessangerSent: " + "Lost user credentials. Please login once more");
                OnMessengerSent(new MessengerSentEventArgs("Lost user credentials. Please login once more."));
            }
            return null;
        }
    }
}
