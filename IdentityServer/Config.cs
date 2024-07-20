using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            ];

        public static IEnumerable<ApiScope> ApiScopes =>
            [
            new ApiScope("grpcservice", "gRPC Service")
            ];

        public static IEnumerable<Client> Clients =>
            [
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "grpcservice" }
            }
            ];

        public static List<TestUser> TestUsers =>
            [
            new TestUser
            {
                SubjectId = "1",
                Username = "alice",
                Password = "password"
            },
            new TestUser
            {
                SubjectId = "2",
                Username = "bob",
                Password = "password"
            }
            ];
    }
}