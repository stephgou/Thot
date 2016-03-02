using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doc.Search.App
{
    public class ThotSettings
    {
        #region Constantes
        public const string API_APP = "https://ndelabarredocsearchapi.azurewebsites.net";

        public const string AAD_TENANT_ID = "delabarrenicolasfree.onmicrosoft.com";
        public const string AAD_BASE_AUTH_URI = "https://login.microsoftonline.com/{0}";
        public const string APP_CLIENT_ID = "58fafee0-b679-42ef-bf25-2c25e12aac35";
        public const string APP_REDIRECT_URI = "http://ndelabarredocsearchapi.azurewebsites.net/.auth/login/done";
        #endregion
    }
}
