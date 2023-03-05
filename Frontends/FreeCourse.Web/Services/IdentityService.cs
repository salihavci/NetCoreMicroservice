using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Abstractions;
using FreeCourse.Web.Inputs;
using FreeCourse.Web.Settings;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Globalization;

namespace FreeCourse.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;

        public IdentityService(HttpClient client, IHttpContextAccessor httpContextAccessor, IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }


        public async Task<TokenResponse> GetAccessTokenByRefreshTokenAsync()
        {
            var discovery = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy() { RequireHttps = false }
            }).ConfigureAwait(false);
            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).ConfigureAwait(false);
            var refreshTokenRequest = new RefreshTokenRequest()
            {
                RefreshToken = refreshToken,
                Address = discovery.TokenEndpoint,
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret
            };

            var token = await _client.RequestRefreshTokenAsync(refreshTokenRequest).ConfigureAwait(false);
            if(token.IsError)
            {
                return null;
            }
            var authenticationTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture) }
            };

            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync().ConfigureAwait(false);
            var authenticationProperties = authenticationResult.Properties;
            authenticationProperties.StoreTokens(authenticationTokens);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,authenticationResult.Principal, authenticationProperties).ConfigureAwait(false);
            return token;
        }

        public async Task RevokeRefreshToken()
        {
            var discovery = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy() { RequireHttps = false }
            }).ConfigureAwait(false);
            if (discovery.IsError)
            {
                throw discovery.Exception;
            }
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken).ConfigureAwait(false);
            var tokenRevokeRequest = new TokenRevocationRequest()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Address = discovery.RevocationEndpoint,
                Token = refreshToken,
                TokenTypeHint = OpenIdConnectParameterNames.RefreshToken
            };

            await _client.RevokeTokenAsync(tokenRevokeRequest).ConfigureAwait(false);
        }

        public async Task<Response<bool>> SignInAsync(SigninInput data)
        {
            var discovery = await _client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = _serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy() { RequireHttps = false}
            }).ConfigureAwait(false);
            if (discovery.IsError)
            {
                throw discovery.Exception;
            }
            var passwordTokenRequest = new PasswordTokenRequest()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = data.Email,
                Password = data.Password,
                Address = discovery.TokenEndpoint
            };

            var token = await _client.RequestPasswordTokenAsync(passwordTokenRequest).ConfigureAwait(false);
            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return Response<bool>.Fail(errorDto.Errors,500);
            }

            var userInfoRequest = new UserInfoRequest()
            {
                Token = token.AccessToken,
                Address = discovery.UserInfoEndpoint
            };

            var userInfo = await _client.GetUserInfoAsync(userInfoRequest).ConfigureAwait(false);
            if(userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            var claims = new ClaimsIdentity(userInfo.Claims,CookieAuthenticationDefaults.AuthenticationScheme,"name","role"); //Tokenden dönen claimlerdeki name ve token'i hangi isimle okuyacağını yazdık
            var claimsPrincipal = new ClaimsPrincipal(claims);
            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken },
                new AuthenticationToken(){ Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture) }
            });

            authenticationProperties.IsPersistent = data.IsRemember;
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,claimsPrincipal, authenticationProperties).ConfigureAwait(false);

            return Response<bool>.Success(true,200);
        }
    }
}
