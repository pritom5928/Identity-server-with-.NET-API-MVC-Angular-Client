﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Client.Controllers
{
    public class AccountController : Controller
    {
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
