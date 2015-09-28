using Doc.Search.API.Models;
using Microsoft.Azure.AppService.ApiApps.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Doc.Search.API.Controllers
{
    public class IdentityController : ApiController
    {
        [HttpGet]
        public async Task<Identity> Get()
        {
            var runtime = Runtime.FromAppSettings(Request);
            var user = runtime.CurrentUser;
            TokenResult token = await user.GetRawTokenAsync("aad");
            var name = (string)token.Claims["name"];
            var email = (string)token.Claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn"];

            return new Identity { EmailAddress = email, Name = name };
        }
    }
}