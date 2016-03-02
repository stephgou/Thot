using Microsoft.Azure.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Threading;
using System.Net.Http.Headers;

namespace Doc.Search.App.Extensions
{
    class TokenExpiredHandler : DelegatingHandler
    {
        private Func<Task<string>> loginDelegate;

        public TokenExpiredHandler(Func<Task<string>> loginDelegate)
        {
            this.loginDelegate = loginDelegate;
        }

        protected override async Task<HttpResponseMessage>
            SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Cloning the request, in case we need to send it again
            var clonedRequest = await CloneRequest(request);
            var response = await base.SendAsync(clonedRequest, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {                    
                    var accessToken = await this.loginDelegate();

                    clonedRequest = await CloneRequest(request);

                    clonedRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    response = await base.SendAsync(clonedRequest, cancellationToken);
                }
                catch (InvalidOperationException)
                {
                    // user cancelled auth, so lets return the original response
                    return response;
                }
            }
            return response;
        }

        private async Task<HttpRequestMessage> CloneRequest(HttpRequestMessage request)
        {
            var result = new HttpRequestMessage(request.Method, request.RequestUri);
            
            foreach (var header in request.Headers)
            {
                result.Headers.Add(header.Key, header.Value);
            }

            if (request.Content != null && request.Content.Headers.ContentType != null)
            {
                var requestBody = await request.Content.ReadAsStringAsync();
                var mediaType = request.Content.Headers.ContentType.MediaType;
                result.Content = new StringContent(requestBody, Encoding.UTF8, mediaType);
                foreach (var header in request.Content.Headers)
                {
                    if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase))
                    {
                        result.Content.Headers.Add(header.Key, header.Value);
                    }
                }
            }
            return result;
        }
    }
}