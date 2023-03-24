using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Gateway.Handlers
{
    public class TokenExchangeDelegateHandler : DelegatingHandler
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public TokenExchangeDelegateHandler(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        private async Task<string> GetToken(string requestToken)
        {
            if(!string.IsNullOrWhiteSpace(_accessToken)) 
            {
                return _accessToken;
            }
            var discovery = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _configuration["IdentityServerUrl"],
                Policy = new DiscoveryPolicy() { RequireHttps = false }
            }).ConfigureAwait(false);
            if (discovery.IsError)
            {
                throw discovery.Exception;
            }
            
            var tokenExchangeTokenRequest = new TokenExchangeTokenRequest()
            {
                Address = discovery.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                GrantType = _configuration["GrantType"],
                SubjectToken = requestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access-token",
                Scope = "openid discount_fullpermission payment_fullpermission"
            };

            var tokenResponse = await _client.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest).ConfigureAwait(false);
            if(tokenResponse.IsError)
            {
                throw tokenResponse.Exception;
            }

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter;
            var newToken = await GetToken(requestToken).ConfigureAwait(false);
            request.SetBearerToken(newToken);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
