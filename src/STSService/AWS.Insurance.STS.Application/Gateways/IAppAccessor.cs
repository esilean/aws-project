using System.Threading.Tasks;

namespace AWS.Insurance.STS.Application.Gateways
{
    public interface IAppAccessor
    {
        Task<bool> ValidateAppCredentials(string appClient, string appSecret);
    }
}
