using Doc.Search.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doc.Search.Portal.Controllers
{
    public class StorageController : Controller
    {
        private static CloudStorageAccount _cloudStorageAccount;
        private static CloudBlobClient _cloudBlobClient;
        private static CloudBlobContainer _cloudBlobContainer;

        private string _searchServiceName;
        private string _searchServiceApiKey;
        private string _indexName;
        private SearchServiceClient _searchServiceClient;
        private SearchIndexClient _searchIndexClient;

        public StorageController()
        {
            var azureStorageConnectionString = CloudConfigurationManager.GetSetting("AzureStorage.ConnectionString");
            var azureStorageContainerReference = CloudConfigurationManager.GetSetting("AzureStorage.ContainerReference");

            _cloudStorageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
            _cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            _cloudBlobContainer = _cloudBlobClient.GetContainerReference(azureStorageContainerReference);

            _searchServiceName = CloudConfigurationManager.GetSetting("SearchServiceName");
            _searchServiceApiKey = CloudConfigurationManager.GetSetting("SearchServiceApiKey");
            _indexName = CloudConfigurationManager.GetSetting("IndexName");
            _searchServiceClient = new SearchServiceClient(_searchServiceName, 
                new SearchCredentials(_searchServiceApiKey));
            _searchIndexClient = _searchServiceClient.Indexes.GetClient(_indexName);
        }

        // GET: Storage
        public ActionResult Index()
        {
            List<DocumentProperties> blobsList = Documents.GetDocumentsFromBlobs(_cloudBlobContainer.ListBlobs(useFlatBlobListing: true));
            return View(blobsList);
        }

        public ActionResult UploadFile()
        {
            var uploadedFiles = Request.Files;
            if (uploadedFiles.Count > 0)
            {

                for (int fileNum = 0; fileNum < uploadedFiles.Count; fileNum++)
                {
                    string fileName = Path.GetFileName(uploadedFiles[fileNum].FileName);
                    if (
                        uploadedFiles[fileNum] != null && uploadedFiles[fileNum].ContentLength > 0)
                    {
                        CloudBlockBlob azureBlockBlob = _cloudBlobContainer.GetBlockBlobReference(fileName);
                        azureBlockBlob.UploadFromStream(uploadedFiles[fileNum].InputStream);
                        azureBlockBlob.Properties.ContentType = uploadedFiles[fileNum].ContentType;
                        azureBlockBlob.SetProperties();

                        var blob = azureBlockBlob as CloudBlob;

                        var doc = DocumentProperties.GetDocFromBlob(blob, azureBlockBlob, "Block");

                        _searchIndexClient.Documents.Index(IndexBatch.Create(IndexAction.Create(doc)));
                    }
                }
                return RedirectToAction("Index");
            }
            return View("UploadFile");
        }
    }
}