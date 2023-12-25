using CompanyEmployees.Client.Models;
using CompanyEmployees.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
