using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc.Search.Model
{
    public class Documents
    {
        public Documents()
        {
        }

        public static List<DocumentProperties> GetDocumentsFromBlobs(IEnumerable<IListBlobItem> blobsList)
        {
            if (blobsList != null && blobsList.Count<IListBlobItem>() > 0)
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
                return documentsList;
            }
            else
                return null;
        }
    }
}
