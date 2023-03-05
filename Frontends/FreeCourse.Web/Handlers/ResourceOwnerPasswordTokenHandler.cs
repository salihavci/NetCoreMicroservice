using FreeCourse.Web.Abstractions;
using FreeCourse.Web.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Web.Handlers
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;
        private IHttpContextAccessor _httpContextAccessor;
        private IIdentityService _identityService;
        private ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        private IHttpContextAccessor httpContextAccessor
        {
            get
            {
                if(_httpContextAccessor == null)
                {
                    _httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
                }
                return _httpContextAccessor;
            }
        }

        private IIdentityService identityService
        {
            get
            {
                if (_identityService == null)
                {
                    _identityService = _serviceProvider.GetRequiredService<IIdentityService>();
                }
                return _identityService;
            }
        }

        private ILogger<ResourceOwnerPasswordTokenHandler> logger
        {
            get
            {
                if(_logger == null)
                {
                    _logger = _serviceProvider.GetRequiredService<ILogger<ResourceOwnerPasswordTokenHandler>>();
                }
                return _logger;
            }
        }


        public ResourceOwnerPasswordTokenHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken).ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await base.SendAsync(request,cancellationToken).ConfigureAwait(false);
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var tokenResponse = await identityService.GetAccessTokenByRefreshTokenAsync().ConfigureAwait(false);
                if(tokenResponse!= null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                    response = await base.SendAsync(request,cancellationToken).ConfigureAwait(false);
                }
            }
            if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnAuthorizeException();
            }
            return response;
        }

    }
}
