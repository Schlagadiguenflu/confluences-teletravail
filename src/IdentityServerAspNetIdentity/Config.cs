// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    UserClaims = { JwtClaimTypes.Role }
                }
            };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                //new ApiResource("api1", "My API #1"),
                new ApiResource("api1", "My API #1", new [] { JwtClaimTypes.Role }),
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                 // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "mvc",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = true,

                    // where to redirect to after login
                    RedirectUris = { Startup.Configuration["URLClientMVC"] + "signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { Startup.Configuration["URLClientMVC"] + "signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1",
                        "roles"
                    },

                    AllowOfflineAccess = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "gestion-stagiaire",
                    ClientName = "Gestion des stagiaires",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 30, // 1 hour = 3600 seconds
                    IdentityTokenLifetime = 30,
                    SlidingRefreshTokenLifetime = 30,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RequireConsent = false,

                    RedirectUris = {
                        Startup.Configuration["URLVueJsGestionStagiaire"] + "callback",
                        Startup.Configuration["URLVueJsGestionStagiaire"] + "static/silent-renew.html"
                    },

                    PostLogoutRedirectUris = { Startup.Configuration["URLVueJsGestionStagiaire"].Remove(Startup.Configuration["URLVueJsGestionStagiaire"].Length - 1) },

                    AllowedCorsOrigins =     { Startup.Configuration["URLVueJsGestionStagiaire"].Remove(Startup.Configuration["URLVueJsGestionStagiaire"].Length - 1) },


                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api1",
                        "roles"
                    },
                }


            };
    }
}