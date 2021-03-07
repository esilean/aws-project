using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Application.Gateways
{
    public interface IAppCredentialsService
    {
        Task<string> GetToken();
    }
}
