using FreeCourse.Web.Abstractions;
using FreeCourse.Web.Settings;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class ClientCredentialTokenService : IClientCredentialTokenService
    {
        private readonly ServiceApiSettings _serviceApiSettings;
        private readonly ClientSettings _clientSettings;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _client;

        public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient client)
        {
            _serviceApiSettings = serviceApiSettings.Value;
            _clientSettings = clientSettings.Value;
            _clientAccessTokenCache = clientAccessTokenCache;
            _client = client;
        }

        public async Task<string> GetToken()
        {
            var currentToken = await _clientAccessTokenCache.GetAsync("WebClientToken").ConfigureAwait(false);
            if(currentToken != null)
            {
                return currentToken.AccessToken;
            }
            var discovery = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy() { RequireHttps = false }
            }).ConfigureAwait(false);
            if (discovery.IsError)
            {
                throw discovery.Exception;
            }
            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = _clientSettings.WebClient.ClientId,
                ClientSecret = _clientSettings.WebClient.ClientSecret,
                Address = discovery.TokenEndpoint
            };
            var newToken = await _client.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest).ConfigureAwait(false);
            if(newToken.IsError)
            {
                throw newToken.Exception;
            }
            await _clientAccessTokenCache.SetAsync("WebClientToken", newToken.AccessToken, newToken.ExpiresIn).ConfigureAwait(false);
            return newToken.AccessToken;
        }
    }
}
