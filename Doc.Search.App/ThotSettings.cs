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
        public const string API_APP = "https://.....azurewebsites.net";

        public const string AAD_TENANT_ID = "Identifiant du tenant Azure AD";
        public const string AAD_BASE_AUTH_URI = "https://login.microsoftonline.com/{0}";
        public const string APP_CLIENT_ID = "Identifiant de l'application universelle";
        public const string APP_REDIRECT_URI = "URL de redirection définie dans Azure AD";
        #endregion
    }
}
