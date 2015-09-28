using Doc.Search.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Doc.Search.API.Controllers
{
    public class SearchController : ApiController
    {
        private string _searchServiceName;
        private string _searchServiceApiKey;
        private string _indexName;
        private SearchServiceClient _searchServiceClient;
        private SearchIndexClient _searchIndexClient;

        public SearchController()
        {
            //var appSettings = ConfigurationManager.AppSettings;
            //var searchServiceName = appSettings["SearchServiceName"];
            //var searchServiceApiKey = appSettings["SearchServiceApiKey"];
            //var indexName = appSettings["IndexName"];
            //var searchServiceName = CloudConfigurationManager.GetSetting("SearchServiceName");
            //var searchServiceApiKey = CloudConfigurationManager.GetSetting("SearchServiceApiKey");
            //var indexName = CloudConfigurationManager.GetSetting("IndexName");
            _searchServiceName = CloudConfigurationManager.GetSetting("SearchServiceName");
            _searchServiceApiKey = CloudConfigurationManager.GetSetting("SearchServiceApiKey");
            _indexName = CloudConfigurationManager.GetSetting("IndexName");
            _searchServiceClient = new SearchServiceClient(_searchServiceName, new SearchCredentials(_searchServiceApiKey));
            _searchIndexClient = _searchServiceClient.Indexes.GetClient(_indexName);
        }


        //http://localhost:15244/api/Search/A
        // double convention
        // name  = nom du param dans webapiConfig
        // GetXxx pour une méthode Get
        // ou alors [HttpGet]
        //http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-and-action-selection

        [HttpGet]
        public IEnumerable<DocumentProperties> SearchbyName(string name)
        {
            try
            {
                DocumentSearchResponse<DocumentProperties> response;
                string filter;
                if (name == "#@!")
                    name = "*";
                if (name.LastIndexOf('*') != name.Length - 1)
                    name += "*";
                if (name == "docx*")
                {
                    filter = "content_type eq 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'";
                    var sp = new SearchParameters();
                    sp.Filter = filter;
                    sp.OrderBy = new List<string>();
                    sp.OrderBy.Add("name");
                    name = "*";
                    response = _searchIndexClient.Documents.Search<DocumentProperties>(name, sp);
                }
                else
                {
                    response = _searchIndexClient.Documents.Search<DocumentProperties>(name);
                }
                List<DocumentProperties> documents = new List<DocumentProperties>();
                foreach (var r in response.Results)
                    documents.Add(r.Document);

                return documents;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Update(string id, Int32 viewsCounter)
        {
            IndexBatch<DocumentProperties> batch =
            IndexBatch.Create(
                IndexAction.Create(
                    IndexActionType.Merge,
                    new DocumentProperties() { id = id, downloads_counter = viewsCounter + 1 }));

            await _searchIndexClient.Documents.IndexAsync(batch);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
