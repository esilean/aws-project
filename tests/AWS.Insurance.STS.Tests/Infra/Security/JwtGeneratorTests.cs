using AWS.Insurance.STS.Infra.Security;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace AWS.Insurance.STS.Tests.Infra.Security
{
    public class JwtGeneratorTests
    {
        [Fact(DisplayName = "Should create a token")]
        public void Infra_JwtGenerator_ShouldCreateAToken()
        {
            // ARRANGE
            var mockOptions = new Mock<IOptions<JwtSettings>>();
            mockOptions.Setup(x => x.Value).Returns(new JwtSettings
            {
                SigningKey = "abcdefghojklmnopqrstuvxwyz",
                Audience = "",
                Issuer = ""
            });

            // ACT
            var sut = new JwtGenerator(mockOptions.Object);
            var token = sut.CreateToken();

            // ASSERT
            Assert.NotNull(token);

        }
    }
}
