using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;

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
                new() {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "grpcservice" }
                }
            ];

        public static List<TestUser> TestUsers =>
            [
                new() {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new() {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            ];
    }
}