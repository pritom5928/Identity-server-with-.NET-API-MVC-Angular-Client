using ISCodeMaze.Configuration;
using ISCodeMaze.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

//services.AddIdentityServer()
//        .AddInMemoryApiScopes(InMemoryConfig.GetApiScopes())
//        .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
//        .AddInMemoryIdentityResources(InMemoryConfig.GetIdentityResources())
//        .AddTestUsers(InMemoryConfig.GetUsers())
//        .AddInMemoryClients(InMemoryConfig.GetClients())
//        .AddDeveloperSigningCredential();


#region new code for tranfering from In-Memory config to ef core databases
var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

services.AddIdentityServer()
    .AddTestUsers(InMemoryConfig.GetUsers())
    .AddDeveloperSigningCredential() //not something we want to use in a production environment
    .AddConfigurationStore(opt =>
    {
        opt.ConfigureDbContext = c => c.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"),
            sql => sql.MigrationsAssembly(migrationAssembly));
    })
    .AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = o => o.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"),
            sql => sql.MigrationsAssembly(migrationAssembly));
    });


#endregion

services.AddControllersWithViews();

var app = builder.Build();

//app.MigrateDatabase();

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

