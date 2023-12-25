using System.Net.Http;
using System.Net.Http.Headers;

namespace CompanyEmployees.Client.Services
{
    public class CompanyHttpClient : ICompanyHttpClient
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private HttpClient _httpClient;

        public CompanyHttpClient(IHttpContextAccessor contextAccessor)
        {
            _httpClient = new HttpClient();
            _contextAccessor = contextAccessor;
        }

        public async Task<HttpClient> GetClient()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7259/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return _httpClient;
        }
    }
}
