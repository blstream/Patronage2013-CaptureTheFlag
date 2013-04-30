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
    class Registration
    {
        private RequestHandler requestHandler;
        private RestRequest request;

        public Registration()
        {
            requestHandler = new RequestHandler();

            request = new RestRequest("/api/players/add", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-type", "application/json");
            request.RequestFormat = DataFormat.Json;
        }

        //* Nazwa użytkownika musi być unikalna.
        private void RequestCallbackOnSuccess(IRestResponse<JsonResponse> response)
        {
            if (response != null)
            {
                if (response.Data != null)
                {
                    Debug.WriteLine("Content: " + response.Content);
                    if (String.IsNullOrEmpty(response.Data.error))
                    {
                        Debug.WriteLine("Response Data retrieved SUCCESSFULY.");
                        Debug.WriteLine("response.Data.error_code: " + response.Data.error_code);
                    }
                    else
                    {
                        Debug.WriteLine("Response Data is an ERROR.");
                        Debug.WriteLine("Exception thrown: " + response.Data.error + ": " + response.Data.error_description);
                        //throw new Exception(response.Data.error + ": " + response.Data.error_description);
                    }
                }
            }
        }

        private void RequestCallbackOnFail(String errorMessage)
        {
            Debug.WriteLine("Exception thrown: " + errorMessage);
            //throw new Exception(errorMessage);
        }

        // TODO: Check if is async
        // Issue: https://tracker.blstreamgroup.com/jira/browse/CTFPAT-88
        public async Task<RestRequestAsyncHandle> Register(UserCredentials user, string verifyPassword)
        {
            if (user == null)
            {
                Debug.WriteLine("Exception thrown: " + "Lost user credentials. Please login once more");
                throw new Exception("Lost user credentials. Please login once more");
            }
            else
            {
                if (user.HasMatchingPassword(verifyPassword))
                {
                    request.AddBody(new { username = user.GetUsername(), password = user.GetPassword() });
                }
                else
                {
                    Debug.WriteLine("Exception thrown: " + "Passwords do not match");
                    throw new Exception("Passwords do not match");
                }
                
            }
            return await requestHandler.ExecuteAsync<JsonResponse>(request, RequestCallbackOnSuccess, RequestCallbackOnFail);
        }
    }
}
