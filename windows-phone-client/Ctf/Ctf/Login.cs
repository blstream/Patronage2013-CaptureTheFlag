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
    class Login
    {
        private RequestHandler requestHandler;
        private RestRequest request;
        private string username;

        public Login()
        {
            requestHandler = new RequestHandler();

            request = new RestRequest("/oauth/token", Method.POST);
            request.AddHeader("Content-type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", "mobile_windows");
            request.AddParameter("grant_type", "password"); 
        }

        public void RequestCallbackOnSuccess(IRestResponse<JsonResponse> response)
        {
            if ( (response != null) && (response.Data != null) )
            {
                    if (String.IsNullOrEmpty(response.Data.error))
                    {
                        if (String.IsNullOrEmpty(response.Data.access_token) && String.IsNullOrEmpty(response.Data.token_type) && String.IsNullOrEmpty(response.Data.scope))
                        {
                            Debug.WriteLine("Response Data is NULL or EMPTY.");
                            Debug.WriteLine("Exception thrown: " + "Unknown error, please try again later.");
                            //throw new Exception("Unknown error, please try again later.");
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
                        Debug.WriteLine("Exception thrown: " + response.Data.error + ": " + response.Data.error_description);
                        //throw new Exception(response.Data.error + ": " + response.Data.error_description);
                    }
            }
        }

        public void RequestCallbackOnFail(String errorMessage)
        {
            Debug.WriteLine("Exception thrown: " + errorMessage);
            //throw new Exception(errorMessage);
        }

        // TODO: Check if is async
        // Issue: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-87
        public async Task<RestRequestAsyncHandle> LogInAs(UserCredentials user, string secret)
        {
            Debug.WriteLine("secret could be: " + System.Guid.NewGuid().ToString());
            if (LoggedAs() != null)
            {
                Debug.WriteLine("Exception thrown: " + "Logged in as another user. Please, logout first");
                throw new Exception("Logged in as another user. Please, logout first");
            }

            if (String.IsNullOrWhiteSpace(secret))
            {
                Debug.WriteLine("Exception thrown: " + "No secret key is set. Please reinstall this app.");
                throw new Exception("No secret key is set. Please reinstall this app.");
            }
            else
                request.AddParameter("client_secret", secret);

            if (user == null)
            {
                Debug.WriteLine("Exception thrown: " + "Lost user credentials. Please login once more");
                throw new Exception("Lost user credentials. Please login once more");
            }
            else
            {
                request.AddParameter("username", user.GetUsername());
                request.AddParameter("password", user.GetPassword());
                username = user.GetUsername();
            }
            return await requestHandler.ExecuteAsync<JsonResponse>(request, RequestCallbackOnSuccess, RequestCallbackOnFail);
        }

        // Issue: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-89
        public User LoggedAs()
        {
            return ApplicationSettings.Instance.RetriveLoggedUser();
        }

        // Issue: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-89
        public bool LogOut()
        {
            return ApplicationSettings.Instance.RemoveFromSettings("user");
        }
    }
}
