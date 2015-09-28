using Doc.Search.App.Extensions;
using Microsoft.Azure.AppService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Doc.Search.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
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

        private IAppServiceClient _appServiceClient;

        public MainViewModel()
        {
            _appServiceClient = new AppServiceClient(
                                ThotSettings.GATEWAY,
                                new TokenExpiredHandler(this.AuthenticateAsync)
                                );
        }
        public async Task<IAppServiceUser> AuthenticateAsync()
        {
            await _appServiceClient.Logout();

            if (_appServiceClient.CurrentUser == null)
            {
                await _appServiceClient.LoginAsync(ThotSettings.AUTH_PROVIDER, false);
            }
            return _appServiceClient.CurrentUser;
        }

        private DocSearchAPI _docSearchAPI;
        public DocSearchAPI DocSearchAPI
        {
            get
            {
                if (_docSearchAPI == null)
                {
                    _docSearchAPI = _appServiceClient.CreateDocSearchAPI(
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