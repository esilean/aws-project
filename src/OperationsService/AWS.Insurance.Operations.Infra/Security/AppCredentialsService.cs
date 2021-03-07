using AWS.Insurance.Operations.Application.Gateways;
using AWS.Insurance.Operations.Infra.Security.Configs;
using AWS.Insurance.Operations.Infra.Security.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Infra.Security
{
    public class AppCredentialsService : IAppCredentialsService
    {
        private readonly ILogger<AppCredentialsService> _logger;
        private readonly HttpClient _client;
        private readonly AppCredentialsSettings _appCredentials;

        public AppCredentialsService(ILogger<AppCredentialsService> logger,
                                     HttpClient client,
                                     IOptions<AppCredentialsSettings> settings)
        {
            _logger = logger;
            _client = client;
            _appCredentials = settings.Value;
        }

        public async Task<string> GetToken()
        {
            var postContent = new
            {
                appClient = _appCredentials.AppClient,
                appSecret = _appCredentials.AppSecret
            };

            var response = await _client.PostAsync(new Uri($"{_appCredentials.BaseAddress}{_appCredentials.AuthEndpoint}"),
                                            new StringContent(JsonConvert.SerializeObject(postContent), Encoding.UTF8, "application/json"));

            var stringContent = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenDto>(stringContent);

            _logger.LogInformation($"Token: {token.Token.Substring(0, 10)}");

            return token.Token;
        }
    }
}
