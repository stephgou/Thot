using System;
using System.Net.Http;
using Microsoft.Azure.AppService;

namespace Doc.Search.App
{
    public static class DocSearchAPIAppServiceExtensions
    {
        public static DocSearchAPI CreateDocSearchAPI(this IAppServiceClient client)
        {
            return new DocSearchAPI(client.CreateHandler());
        }

        public static DocSearchAPI CreateDocSearchAPI(this IAppServiceClient client, params DelegatingHandler[] handlers)
        {
            return new DocSearchAPI(client.CreateHandler(handlers));
        }

        public static DocSearchAPI CreateDocSearchAPI(this IAppServiceClient client, Uri uri, params DelegatingHandler[] handlers)
        {
            return new DocSearchAPI(uri, client.CreateHandler(handlers));
        }

        public static DocSearchAPI CreateDocSearchAPI(this IAppServiceClient client, HttpClientHandler rootHandler, params DelegatingHandler[] handlers)
        {
            return new DocSearchAPI(rootHandler, client.CreateHandler(handlers));
        }
    }
}
