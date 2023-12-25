using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaneesController : ControllerBase
    {
        private List<Company> _companies = new List<Company>();
        public CompaneesController()
        {
            _companies.AddRange(new List<Company> {
            new Company
            {
                Id = 1,
                Name= "Pritom com"
            },
            new Company
            {
                Id = 2,
                Name = "Jugal com"
            }});
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var dd = User.Claims;
            return Ok(_companies);
        }
    }


    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
