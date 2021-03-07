using AWS.Insurance.STS.Application.Gateways;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWS.Insurance.STS.Infra.AppAcessor
{
    public class AppAcessorFake : IAppAccessor
    {
        private readonly Dictionary<string, string> AppClients;

        public AppAcessorFake(IOptions<AppAccessorSettings> settings)
        {
            AppClients = new Dictionary<string, string>();

            foreach (var appCreds in settings.Value.Credentials)
            {
                var appCred = appCreds.Split(":");
                AppClients.Add(appCred[0], appCred[1]);
            }
        }

        public async Task<bool> ValidateAppCredentials(string appClient, string appSecret)
        {
            var clientExists = AppClients.ContainsKey(appClient);
            if (clientExists)
                return await Task.FromResult(AppClients[appClient] == appSecret);

            return false;
        }
    }
}
