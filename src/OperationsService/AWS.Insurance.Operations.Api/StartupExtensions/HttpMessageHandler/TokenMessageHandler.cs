using AWS.Insurance.Operations.Application.Gateways;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Api.StartupExtensions.HttpMessageHandler
{
    public class TokenMessageHandler : DelegatingHandler
    {

        private readonly ILogger<TokenMessageHandler> _logger;
        private readonly IAppCredentialsService _appCredentialsService;

        public TokenMessageHandler(ILogger<TokenMessageHandler> logger,
                                   IAppCredentialsService appCredentialsService)
        {
            _logger = logger;
            _appCredentialsService = appCredentialsService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding Token...");

            var token = await _appCredentialsService.GetToken();
            request.Headers.Add("Authorization", $"Bearer {token}");
            return await base.SendAsync(request, cancellationToken);
        }

    }
}
