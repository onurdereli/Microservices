using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Course.Web.Exceptions;
using Course.Web.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Course.Web.Handler
{
    public class ResourceOwnerPasswordTokenHandler: DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IIdentityService _identityService;

        private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

        public ResourceOwnerPasswordTokenHandler(IIdentityService identityService, IHttpContextAccessor httpContextAccessor, ILogger<ResourceOwnerPasswordTokenHandler> logger)
        {
            _identityService = identityService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            if(httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                var accessTokenByRefleshToken = await _identityService.GetAccessTokenByRefleshToken();

                if(accessTokenByRefleshToken != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenByRefleshToken.AccessToken);
                    
                    httpResponseMessage = await base.SendAsync(request, cancellationToken);
                }
            }

            if(httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnAuthorizeException();
            }

            return httpResponseMessage;
        }
    }
}
