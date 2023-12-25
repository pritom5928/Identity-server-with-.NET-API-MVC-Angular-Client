using CompanyEmployees.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


// Add services to the container.
services.AddRazorPages();

#region identity server related setup
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
services.AddScoped<ICompanyHttpClient, CompanyHttpClient>();


services.AddAuthentication(opt =>
{
    opt.DefaultScheme = "Cookies";
    opt.DefaultChallengeScheme = "oidc";
}).AddCookie("Cookies")
.AddOpenIdConnect("oidc", opt =>
{
    opt.SignInScheme = "Cookies";
    opt.Authority = "https://localhost:7297";
    opt.ClientId = "mvc-client";
    opt.ResponseType = "code id_token";
    opt.SaveTokens = true;
    opt.ClientSecret = "MVCSecret";
    opt.GetClaimsFromUserInfoEndpoint =true;

    opt.ClaimActions.DeleteClaim("sid");
    opt.ClaimActions.DeleteClaim("idp");

    opt.Scope.Add("address");

    opt.Scope.Add("roles");
    opt.ClaimActions.MapUniqueJsonKey("role", "role");


    opt.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "role"
    };

    //opt.ClaimActions.MapUniqueJsonKey("address", "address");
});
#endregion

services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

//app.MapRazorPages();

app.Run();
