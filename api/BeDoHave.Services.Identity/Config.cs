// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace BeDoHave.Services.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                        new IdentityResources.OpenId(),
                        new IdentityResources.Profile(),
                   };

        // DONE (1) Add ApiResources for different microservices
        // API itself
        public static IEnumerable<ApiResource> ApiResources =>
                    new ApiResource[]
                    {
                        new ApiResource("bedohave", "BeDoHave APIs")
                        {
                            Scopes = { "bedohave.fullaccess" }
                        }
                    };


        // Something within API
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("bedohave.fullaccess")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "bedohave.fullaccess" }
                },

                new Client {
                    ClientId = "angular_spa",
                    ClientName = "Angular 4 Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes = new List<string> {"openid", "profile", "bedohave.fullaccess"},
                    RedirectUris = new List<string> {"http://localhost:8100/auth-callback"},
                    PostLogoutRedirectUris = new List<string> {"http://localhost:8100/"},
                    AllowedCorsOrigins = new List<string> {"http://localhost:8100"},
                    AllowAccessTokensViaBrowser = true
                }
            };
    }
}