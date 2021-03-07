namespace AWS.Insurance.STS.Application.Gateways
{
    public interface IJwtGenerator
    {
        string CreateToken();
    }
}
