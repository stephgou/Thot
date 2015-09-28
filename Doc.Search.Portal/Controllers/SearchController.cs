using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System.Text;
using Microsoft.Azure;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Threading;
using Microsoft.WindowsAzure;
using Doc.Search.Model;
//using System.Web.Http;

namespace Doc.Search.Portal.Controllers
{
    [RoutePrefix("")]
    public class SearchController : Controller
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
            _searchServiceClient = new SearchServiceClient(_searchServiceName, 
                                        new SearchCredentials(_searchServiceApiKey));
            _searchIndexClient = _searchServiceClient.Indexes.GetClient(_indexName);
        }

        [Route()]
        public ActionResult Index()
        {
            return View();
        }

        [Route("search")]
        public ActionResult Search(string search)
        {
            try
            {
                DocumentSearchResponse<DocumentProperties> response;
                string filter;
                if (search.LastIndexOf('*') != search.Length - 1)
                    search += "*";
                if (search == "docx*")
                {
                    filter = "content_type eq 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'";
                    var sp = new SearchParameters();
                    sp.Filter = filter;
                    search = "*";
                    response = _searchIndexClient.Documents.Search<DocumentProperties>(search, sp);
                }
                else
                {
                    response = _searchIndexClient.Documents.Search<DocumentProperties>(search);
                }

                var r = response.Results;
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var content = serializer.Serialize(r);
                return Content(content);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
        }

        [Route("counter")]
        public async Task<ActionResult> Update(string id, Int32 viewsCounter)
        {
            IndexBatch<DocumentProperties> batch =
            IndexBatch.Create(
                IndexAction.Create(
                    IndexActionType.Merge,
                    new DocumentProperties() { id = id, downloads_counter = viewsCounter + 1 }));

            await _searchIndexClient.Documents.IndexAsync(batch);

            //Wait for index update before new request...
            Thread.Sleep(1000);
            return Content("OK");
        }

    }
}