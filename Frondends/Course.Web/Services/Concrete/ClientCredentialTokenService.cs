using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Course.Web.Models;
using Course.Web.Services.Abstract;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Course.Web.Services.Concrete
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly HttpClient _httpClient;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;

        public ClientCredentialTokenService(HttpClient httpClient, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings, IClientAccessTokenCache clientAccessTokenCache)
        {
            _httpClient = httpClient;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
        }

        public async Task<string> GetToken()
        {
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken");

            if (currentToken != null)
            {
                return currentToken.AccessToken;
            }

            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            ClientCredentialsTokenRequest clientCredentialsTokenRequest = new()
            {
                ClientId = _clientSettings.WebClient.ClientId,
                ClientSecret = _clientSettings.WebClient.ClientSecret,
                Address = discovery.TokenEndpoint
            };

            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

            if (newToken.IsError)
            {
                throw newToken.Exception;
            }

            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn);

            return newToken.AccessToken;
        }
    }
}
