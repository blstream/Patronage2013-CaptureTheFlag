using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctf
{
    /// <summary>
    /// 
    /// </summary>
    class RequestHandler
    {
        private RestClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandler"/> class.
        /// </summary>
        public RequestHandler()
        {
            client = new RestClient("http://capturetheflag.blstream.com:18080/demo");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestHandler"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public RequestHandler(String baseUrl)
        {
            client = new RestClient(baseUrl);
        }

        /// <summary>
        /// Executes the async.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request">The request.</param>
        /// <param name="CallbackOnSuccess">The callback on success.</param>
        /// <param name="CallbackOnFail">The callback on fail.</param>
        /// <returns></returns>
        public async Task<RestRequestAsyncHandle> ExecuteAsync<T>(RestRequest request, Action<IRestResponse<T>> CallbackOnSuccess, Action<String> CallbackOnFail) where T : new()
        {
            RestRequestAsyncHandle asyncHandler = client.ExecuteAsync<T>(request, (response) =>
                {
                    if (response.ResponseStatus == ResponseStatus.Error)
                    {
                        CallbackOnFail(response.ErrorMessage);
                    }
                    else
                    {
                        CallbackOnSuccess(response);
                    }
                });

            return asyncHandler;
        }
    }
}
