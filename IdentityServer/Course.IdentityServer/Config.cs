// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityServer4;

namespace Course.IdentityServer
{
    // NOT: Token dağıtıcı IdentityServer - Alan MVC olacak
    //Client Credentials Grant Type > Üyelik sistemi gerektirmeyen
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes = {"catolog_fullpermission"}},
            new ApiResource("photo_stock_catalog"){Scopes = {"photo_stock_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        //Üyelikle ilgili işlemler
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       //her eklenen bir claim'e karşılık gelir
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(), //mutlaka olmalı
                       new IdentityResources.Profile(),
                       new IdentityResource{Name = "roles", DisplayName = "Roles", Description = "Kullanıcı Rolleri", UserClaims = new List<string>{"role"}}
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catolog_fullpermission","Catalog API için full erişim"),
                new ApiScope("photo_stock_fullpermission","Photo Stock API için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "WebMvcClient",
                    ClientName = "Asp.Net Core MVC",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = { GrantType.ClientCredentials },
                    AllowedScopes = { "catolog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client
                {
                    ClientId = "WebMvcClientForUser",
                    ClientName = "Asp.Net Core MVC",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowOfflineAccess = true,
                    // reflesh token oluşturmak için "ResourceOwnerPassword" kulllanılır
                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                    // OpenId mutlaka olmalı - Scopelar servislerin token ile erişebileceği bilgileri tutar
                    // IdentityServer'a istek yapabilmek için "IdentityServerConstants.LocalApi.ScopeName" ekli olmalı
                    AllowedScopes = { IdentityServerConstants.LocalApi.ScopeName, IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, "roles"  },
                    AccessTokenLifetime = 1*60*60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                    RefreshTokenUsage = TokenUsage.ReUse 
                    // Mantığı; 60 gün boyunca siteye 1 kez bile girilmezse login ekranına geri döndürür
                }
            };
    }
}