using Doc.Search.Model;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BuildIndex.Search.App
{
    class Program
    {
        private static SearchServiceClient _searchServiceClient;
        private static SearchIndexClient _searchIndexClient;

        static void Main(string[] args)
        {

            Console.WriteLine("Création et mise à jour de l'index <docs> à partir du storage <docsearch>...");
            Console.WriteLine("----------------------------------------------------------------------------");

            var appSettings = ConfigurationManager.AppSettings;

            var azureStorageConnectionString = appSettings["AzureStorage.ConnectionString"];
            var indexName = appSettings["IndexName"];
            var searchCriteria = appSettings["SearchCriteria"];

            var searchServiceName = appSettings["SearchServiceName"];
            var searchServiceApiKey = appSettings["SearchServiceApiKey"];

            // Create an HTTP reference to the catalog index
            _searchServiceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(searchServiceApiKey));

            Console.WriteLine("Suppression de l'index <"+ indexName + ">...");
            DeleteIndex(indexName);
            Console.WriteLine("Création de l'index <" + indexName + ">...");
            CreateIndex(indexName);

            Console.WriteLine("Mise à jour de l'index<" + indexName + "> à partir du storage <docsearch>...");
            _searchIndexClient = _searchServiceClient.Indexes.GetClient(indexName);
            UploadDocuments(azureStorageConnectionString);

            Console.WriteLine("Recherche des documents sur le critère : " + searchCriteria);
            Search(searchText: searchCriteria);

            Console.WriteLine("\n{0}", "Filtre des documents ayant comme extension 'docx'...\n");

            Search(searchText: "*", filter: "content_type eq 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'");
            
            Console.ReadLine();
        }

        private static void CreateIndex(string IndexName)
        {
            try
            {
                var definition = new Index()
                {
                    Name = IndexName,
                    Fields = new[] 
                    { 
                        new Field("id", DataType.String)                    { IsKey = true,  IsSearchable = true,  IsFilterable = true, IsSortable = true, IsFacetable = false, IsRetrievable = true},
                        new Field("url", DataType.String)                   { IsKey = false, IsSearchable = true,  IsFilterable = true, IsSortable = true, IsFacetable = false, IsRetrievable = true},
                        new Field("name", DataType.String)                  { IsKey = false, IsSearchable = true,  IsFilterable = true, IsSortable = true, IsFacetable = false, IsRetrievable = true},
                        new Field("blob_type", DataType.String)             { IsKey = false, IsSearchable = true,  IsFilterable = true, IsSortable = true, IsFacetable = true,  IsRetrievable = true},
                        new Field("content_type", DataType.String)          { IsKey = false, IsSearchable = true,  IsFilterable = true, IsSortable = true, IsFacetable = true,  IsRetrievable = true},
                        new Field("size", DataType.Double)                  { IsKey = false, IsSearchable = false, IsFilterable = true, IsSortable = true, IsFacetable = false, IsRetrievable = true},
                        new Field("last_modified", DataType.DateTimeOffset) { IsKey = false, IsSearchable = false, IsFilterable = true, IsSortable = true, IsFacetable = false, IsRetrievable = true},
                        new Field("container", DataType.String)             { IsKey = false, IsSearchable = true,  IsFilterable = true, IsSortable = true, IsFacetable = true,  IsRetrievable = true},
                        new Field("downloads_counter", DataType.Int32)      { IsKey = false, IsSearchable = false, IsFilterable = true, IsSortable = true, IsFacetable = false, IsRetrievable = true}
                    }
                };
                _searchServiceClient.Indexes.Create(definition);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la création de l'index: {0}\r\n", ex.Message.ToString());
            }
        }

        private static bool DeleteIndex(string IndexName)
        {
            // Delete the index if it exists
            try
            {
                if (_searchServiceClient.Indexes.Exists(IndexName))
                {
                    _searchServiceClient.Indexes.Delete(IndexName);
                }
                _searchServiceClient.Indexes.Delete(IndexName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting index: {0}\r\n", ex.Message.ToString());
                Console.WriteLine("Did you remember to add your SearchServiceName and SearchServiceApiKey to the app.config?\r\n");
                return false;
            }
            return true;
        }

        private static void UploadDocuments(string azureStorageConnectionString)
        {

            var storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            BlobContinuationToken continuationToken = null;
            BlobContinuationToken containerContinuationToken = null;
            List<string> containerNames = new List<string>();
            long totalBlobsIndexed = 0;
            do
            {
                var fetchContainersTask = Task.Run<ContainerResultSegment>(async () =>
                {
                    return await blobClient.ListContainersSegmentedAsync("", ContainerListingDetails.All, 100, containerContinuationToken, null, null);
                });
                var fetchContainersTaskResult = fetchContainersTask.Result;
                containerContinuationToken = fetchContainersTaskResult.ContinuationToken;
                var containers = fetchContainersTaskResult.Results.ToList();
                foreach (var container in containers)
                {
                    Console.WriteLine("Affichage de la liste des blobs depuis '" + container.Name + "' et mise à jour de l'index de search service.");
                    long totalBlobsUploaded = 0;
                    continuationToken = null;
                    do
                    {
                        var fetchBlobsTask = Task.Run<BlobResultSegment>(async () =>
                        {
                            return await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 100, continuationToken, null, null);
                        });
                        var blobListingResult = fetchBlobsTask.Result;
                        continuationToken = blobListingResult.ContinuationToken;
                        var blobsList = blobListingResult.Results.ToList();
                        if (blobsList.Count > 0)
                        {
                            var documentsList = new List<DocumentProperties>();

                            foreach (var blob in blobsList)
                            {
                                var blockBlob = blob as CloudBlockBlob;

                                if (blockBlob != null)
                                {
                                    var doc = DocumentProperties.GetDocFromBlob(blob, blockBlob, "Block");
                                    documentsList.Add(doc);
                                }
                                var pageBlob = blob as CloudPageBlob;
                                if (pageBlob != null)
                                {
                                    var doc = DocumentProperties.GetDocFromBlob(blob, blockBlob, "Page");
                                    documentsList.Add(doc);
                                }

                            }

                            try
                            {
                                _searchIndexClient.Documents.Index(IndexBatch.Create(documentsList.Select(eachDoc => IndexAction.Create(eachDoc))));
                            }
                            catch (IndexBatchException e)
                            {
                                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                                // the batch. Depending on your application, you can take compensating actions like delaying and
                                // retrying. For this simple demo, we just log the failed document keys and continue.
                                Console.WriteLine(
                                    "Echec lors de l'indexation des documents: {0}",
                                    String.Join(", ", e.IndexResponse.Results.Where(r => !r.Succeeded).Select(r => r.Key)));
                            }

                            // Wait a while for indexing to complete.
                            Thread.Sleep(2000);

                            totalBlobsUploaded += documentsList.Count;
                            totalBlobsIndexed += documentsList.Count;

                        }
                    }
                    while (continuationToken != null);
                    Console.WriteLine(totalBlobsUploaded + " blobs téléchargés depuis le container '" + container.Name + "'.");
                    Console.WriteLine("---------------------------------------------------------------------------------------");
                }

            } while (containerContinuationToken != null);
            Console.WriteLine(totalBlobsIndexed + " documents téléchargés depuis le compte de stockage '" + azureStorageConnectionString + "'.");
        }


        private static void Search(string searchText, string filter = null)
        {
            if (searchText.LastIndexOf('*') != searchText.Length-1)
                searchText += "*";

            if (searchText == "docx*")
            {
                filter = "content_type eq 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'";
                searchText = "*";
            }

            var sp = new SearchParameters();

            if (!String.IsNullOrEmpty(filter))
            {
                sp.Filter = filter;
            }

            DocumentSearchResponse<DocumentProperties> response = 
                _searchIndexClient.Documents.Search<DocumentProperties>(searchText, sp);
            foreach (SearchResult<DocumentProperties> result in response)
            {
                Console.WriteLine(result.Document.name);
            }
        }
    }
}
