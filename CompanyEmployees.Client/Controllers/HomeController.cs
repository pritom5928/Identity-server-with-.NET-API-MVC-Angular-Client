using CompanyEmployees.Client.Models;
using CompanyEmployees.Client.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Security.Claims;

namespace CompanyEmployees.Client.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICompanyHttpClient _companyHttpClient;
        public HomeController(ICompanyHttpClient companyHttpClient)
        {
            _companyHttpClient = companyHttpClient;
        }

        public async Task<IActionResult> Index()
        {
            var httpClient = await _companyHttpClient.GetClient();

            var response = await httpClient.GetAsync("api/companees").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var companiesString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                var companyViewModel = JsonConvert.DeserializeObject<List<CompanyViewModel>>(companiesString).ToList();

                return View(companyViewModel);
            }

            throw new Exception($"Problem with fetching data from the API: {response.ReasonPhrase}");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Privacy()
        {
            var client = new HttpClient();
            var metaDataResponse = await client.GetDiscoveryDocumentAsync("https://localhost:7297");

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = metaDataResponse.UserInfoEndpoint,
                Token = accessToken
            });

            if (response.IsError)
            {
                throw new Exception("Problem while fetching data from the UserInfo endpoint", response.Exception);
            }

            var addressClaim = response.Claims.FirstOrDefault(f => f.Type.Equals("address"));

            User.AddIdentity(new ClaimsIdentity(new List<Claim> { new Claim(addressClaim.Type.ToString(), addressClaim.Value.ToString()) }));
          
            return View();
        }
    }
}
