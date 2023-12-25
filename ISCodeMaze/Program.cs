using ISCodeMaze.Configuration;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddIdentityServer()
        .AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
        .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
        .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
        .AddTestUsers(InMemoryConfig.GetUsers())
        .AddInMemoryClients(InMemoryConfig.GetClients())
        .AddDeveloperSigningCredential();

services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();

//app.MapGet("/", () => "Hello World!");

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();

