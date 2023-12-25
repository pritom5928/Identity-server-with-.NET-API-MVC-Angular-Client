using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace ISCodeMaze.Configuration
{
    public static class InMemoryConfig
    {
        //identity resources
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
          new List<IdentityResource>
          {
             new IdentityResources.OpenId(),
             new IdentityResources.Profile()
          };

        //users
        public static List<TestUser> GetUsers() =>
          new List<TestUser>
          {
              new TestUser
              {
                  SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
                  Username = "Mick",
                  Password = "MickPassword",
                  Claims = new List<Claim>
                  {
                      new Claim("given_name", "Mick"),
                      new Claim("family_name", "Mining")
                  }
              },
              new TestUser
              {
                  SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
                  Username = "Jane",
                  Password = "JanePassword",
                  Claims = new List<Claim>
                  {
                      new Claim("given_name", "Jane"),
                      new Claim("family_name", "Downing")
                  }
              }
          };

        //clients
        public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
           new Client
           {
                ClientId = "company-employee",
                ClientSecrets = new [] { new Secret("codemazesecret".Sha512()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "companyApi" }
           },
           new Client
           {
                 ClientName = "MVC Client",
                 ClientId = "mvc-client",
                 AllowedGrantTypes = GrantTypes.Hybrid,
                 RedirectUris = new List<string>{ "https://localhost:7031/signin-oidc" },
                 RequirePkce = false,
                 AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId,       IdentityServerConstants.StandardScopes.Profile },
                 ClientSecrets = { new Secret("MVCSecret".Sha512()) },
                 PostLogoutRedirectUris = new List<string> { "https://localhost:7031/signout-callback-oidc" }
           }
        };

        //api scopes
        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope> { new ApiScope("companyApi", "CompanyEmployee API") };

        public static IEnumerable<ApiResource> GetApiResources() =>
        new List<ApiResource>
        {
            new ApiResource("companyApi", "CompanyEmployee API")
            {
                Scopes = { "companyApi" }
            }
        };
    }
}
