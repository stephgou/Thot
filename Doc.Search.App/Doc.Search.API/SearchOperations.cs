﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Doc.Search.App;
using Doc.Search.App.Models;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;

namespace Doc.Search.App
{
    internal partial class SearchOperations : IServiceOperations<DocSearchAPI>, ISearchOperations
    {
        /// <summary>
        /// Initializes a new instance of the SearchOperations class.
        /// </summary>
        /// <param name='client'>
        /// Reference to the service client.
        /// </param>
        internal SearchOperations(DocSearchAPI client)
        {
            this._client = client;
        }
        
        private DocSearchAPI _client;
        
        /// <summary>
        /// Gets a reference to the Doc.Search.App.DocSearchAPI.
        /// </summary>
        public DocSearchAPI Client
        {
            get { return this._client; }
        }
        
        /// <param name='name'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        public async Task<HttpOperationResponse<IList<DocumentProperties>>> SearchbyNameWithOperationResponseAsync(string name, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // Validate
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            
            // Tracing
            bool shouldTrace = ServiceClientTracing.IsEnabled;
            string invocationId = null;
            if (shouldTrace)
            {
                invocationId = ServiceClientTracing.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("name", name);
                ServiceClientTracing.Enter(invocationId, this, "SearchbyNameAsync", tracingParameters);
            }
            
            // Construct URL
            string url = "";
            url = url + "/api/Search/";
            url = url + Uri.EscapeDataString(name);
            string baseUrl = this.Client.BaseUri.AbsoluteUri;
            // Trim '/' character from the end of baseUrl and beginning of url.
            if (baseUrl[baseUrl.Length - 1] == '/')
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
            url = baseUrl + "/" + url;
            url = url.Replace(" ", "%20");
            
            // Create HTTP transport objects
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Get;
            httpRequest.RequestUri = new Uri(url);
            
            // Set Credentials
            if (this.Client.Credentials != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await this.Client.Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            }
            
            // Send Request
            if (shouldTrace)
            {
                ServiceClientTracing.SendRequest(invocationId, httpRequest);
            }
            cancellationToken.ThrowIfCancellationRequested();
            HttpResponseMessage httpResponse = await this.Client.HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            if (shouldTrace)
            {
                ServiceClientTracing.ReceiveResponse(invocationId, httpResponse);
            }
            HttpStatusCode statusCode = httpResponse.StatusCode;
            cancellationToken.ThrowIfCancellationRequested();
            string responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (statusCode != HttpStatusCode.OK)
            {
                HttpOperationException<object> ex = new HttpOperationException<object>();
                ex.Request = httpRequest;
                ex.Response = httpResponse;
                ex.Body = null;
                if (shouldTrace)
                {
                    ServiceClientTracing.Error(invocationId, ex);
                }
                throw ex;
            }
            
            // Create Result
            HttpOperationResponse<IList<DocumentProperties>> result = new HttpOperationResponse<IList<DocumentProperties>>();
            result.Request = httpRequest;
            result.Response = httpResponse;
            
            // Deserialize Response
            if (statusCode == HttpStatusCode.OK)
            {
                IList<DocumentProperties> resultModel = new List<DocumentProperties>();
                JToken responseDoc = null;
                if (string.IsNullOrEmpty(responseContent) == false)
                {
                    responseDoc = JToken.Parse(responseContent);
                }
                if (responseDoc != null)
                {
                    resultModel = DocumentPropertiesCollection.DeserializeJson(responseDoc);
                }
                result.Body = resultModel;
            }
            
            if (shouldTrace)
            {
                ServiceClientTracing.Exit(invocationId, result);
            }
            return result;
        }
        
        /// <param name='id'>
        /// Required.
        /// </param>
        /// <param name='viewsCounter'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        public async Task<HttpOperationResponse<string>> UpdateWithOperationResponseAsync(string id, int viewsCounter, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // Validate
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            
            // Tracing
            bool shouldTrace = ServiceClientTracing.IsEnabled;
            string invocationId = null;
            if (shouldTrace)
            {
                invocationId = ServiceClientTracing.NextInvocationId.ToString();
                Dictionary<string, object> tracingParameters = new Dictionary<string, object>();
                tracingParameters.Add("id", id);
                tracingParameters.Add("viewsCounter", viewsCounter);
                ServiceClientTracing.Enter(invocationId, this, "UpdateAsync", tracingParameters);
            }
            
            // Construct URL
            string url = "";
            url = url + "/api/Search";
            List<string> queryParameters = new List<string>();
            queryParameters.Add("id=" + Uri.EscapeDataString(id));
            queryParameters.Add("viewsCounter=" + Uri.EscapeDataString(viewsCounter.ToString()));
            if (queryParameters.Count > 0)
            {
                url = url + "?" + string.Join("&", queryParameters);
            }
            string baseUrl = this.Client.BaseUri.AbsoluteUri;
            // Trim '/' character from the end of baseUrl and beginning of url.
            if (baseUrl[baseUrl.Length - 1] == '/')
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
            url = baseUrl + "/" + url;
            url = url.Replace(" ", "%20");
            
            // Create HTTP transport objects
            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Put;
            httpRequest.RequestUri = new Uri(url);
            
            // Set Credentials
            if (this.Client.Credentials != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await this.Client.Credentials.ProcessHttpRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            }
            
            // Send Request
            if (shouldTrace)
            {
                ServiceClientTracing.SendRequest(invocationId, httpRequest);
            }
            cancellationToken.ThrowIfCancellationRequested();
            HttpResponseMessage httpResponse = await this.Client.HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            if (shouldTrace)
            {
                ServiceClientTracing.ReceiveResponse(invocationId, httpResponse);
            }
            HttpStatusCode statusCode = httpResponse.StatusCode;
            cancellationToken.ThrowIfCancellationRequested();
            string responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (statusCode != HttpStatusCode.OK)
            {
                HttpOperationException<object> ex = new HttpOperationException<object>();
                ex.Request = httpRequest;
                ex.Response = httpResponse;
                ex.Body = null;
                if (shouldTrace)
                {
                    ServiceClientTracing.Error(invocationId, ex);
                }
                throw ex;
            }
            
            // Create Result
            HttpOperationResponse<string> result = new HttpOperationResponse<string>();
            result.Request = httpRequest;
            result.Response = httpResponse;
            
            // Deserialize Response
            if (statusCode == HttpStatusCode.OK)
            {
                string resultModel = default(string);
                JToken responseDoc = null;
                if (string.IsNullOrEmpty(responseContent) == false)
                {
                    responseDoc = JToken.Parse(responseContent);
                }
                if (responseDoc != null)
                {
                    resultModel = responseDoc.ToString(Newtonsoft.Json.Formatting.Indented);
                }
                result.Body = resultModel;
            }
            
            if (shouldTrace)
            {
                ServiceClientTracing.Exit(invocationId, result);
            }
            return result;
        }
    }
}
