using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc.Search.Model
{
    public class DocumentProperties
    {
        public string id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string blob_type { get; set; }
        public string content_type { get; set; }
        public Double size { get; set; }
        public DateTimeOffset last_modified { get; set; }
        public String container { get; set; }
        public Int32 downloads_counter { get; set; }

        public static DocumentProperties GetDocFromBlob(IListBlobItem blob, ICloudBlob cloudBlob, string blobType)
        {
            if (cloudBlob.Properties != null)
            {
                return new DocumentProperties()
                {
                    id = string.Format("id{0}", blob.Uri.AbsoluteUri.GetHashCode()),
                    url = blob.Uri.AbsoluteUri,
                    name = cloudBlob.Name,
                    blob_type = blobType,
                    content_type = cloudBlob.Properties.ContentType,
                    size = cloudBlob.Properties.Length,
                    last_modified = cloudBlob.Properties.LastModified.Value,
                    container = blob.Container.Name,
                    downloads_counter = 1
                };
            }
            else
                return null;

        }
    }
}
