using Doc.Search.App.Extensions;
using Microsoft.Azure.AppService;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Doc.Search.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Création de l'URL d'accès au service d'authentification Azure AD
        static string authority = String.Format(CultureInfo.InvariantCulture, 
                                                ThotSettings.AAD_BASE_AUTH_URI, 
                                                ThotSettings.AAD_TENANT_ID);

        private AuthenticationContext authContext = null;

        private string _userName = "";
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyPropertyChanged(() => UserName);
            }
        }

        public string Token { get; set; }

        //private IAppServiceClient _appServiceClient;

        public MainViewModel()
        {
            authContext = new AuthenticationContext(authority);
            Token = null;
        }       

        public async Task<string> AuthenticateAsync()
        {
            if (Token == null)
            {
                // Déclenchement de l'authentification de l'utilisateur auprès du tenant Azure AD en utilisant l'URL de la
                // ressource accédée (URL de l'API App Doc.Search.API) ainsi que les identifiants AAD (Client ID et URI de redirection)
                // de l'application native
                AuthenticationResult result = await authContext.AcquireTokenAsync(  ThotSettings.API_APP, 
                                                                                    ThotSettings.APP_CLIENT_ID, 
                                                                                    new Uri(ThotSettings.APP_REDIRECT_URI));

                if (result.Status != AuthenticationStatus.Success)
                {
                    if (result.Error == "authentication_canceled")
                    {
                        // The user cancelled the sign-in, no need to display a message.
                    }
                    else
                    {
                        MessageDialog dialog = new MessageDialog(string.Format("If the error continues, please contact your administrator.\n\nError: {0}\n\nError Description:\n\n{1}", result.Error, result.ErrorDescription), "Sorry, an error occurred while signing you in.");
                        await dialog.ShowAsync();
                    }
                    return string.Empty;
                }

                Token = result.AccessToken;
            }

            return Token;
        }

        private DocSearchAPI _docSearchAPI;
        public DocSearchAPI DocSearchAPI
        {
            get
            {
                if (_docSearchAPI == null)
                {
                    _docSearchAPI = new DocSearchAPI(
                                            new Uri(ThotSettings.API_APP),
                                            new TokenExpiredHandler(AuthenticateAsync)
                    );
                }
                return _docSearchAPI;
            }
            set { _docSearchAPI = value; }
        }

        public async void GetUserName()
        {
            try
            {
                var response = await DocSearchAPI.Identity.GetAsync();
                var userEmail = response.Name;
                string[] substrings = Regex.Split(userEmail, "@");
                if (substrings.Length != 0)
                    UserName = substrings[0];
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}