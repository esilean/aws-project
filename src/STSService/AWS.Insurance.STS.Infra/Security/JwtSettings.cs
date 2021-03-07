namespace AWS.Insurance.STS.Infra.Security
{
    public class JwtSettings
    {
        public string SigningKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}
