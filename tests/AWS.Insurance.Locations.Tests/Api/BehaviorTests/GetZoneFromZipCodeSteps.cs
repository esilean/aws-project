using AWS.Insurance.Locations.Api;
using AWS.Insurance.Locations.Tests.Api.AConfig;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace AWS.Insurance.Locations.Tests.Api.BehaviorTests
{
    [Binding]
    public class GetZoneFromZipCodeSteps
    {
        private readonly ScenarioContext _context;
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public GetZoneFromZipCodeSteps(ScenarioContext context)
        {
            _context = context;
        }

        public GetZoneFromZipCodeSteps(ScenarioContext context,
                                       IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;
            _context = context;
        }

        [When(@"I call the LocationApi passing a ZipCode: '(.*)'")]
        public async Task WhenICallTheLocationApiPassingAZipCode(int zipCode)
        {
            var response = await _testsFixture.Client.GetAsync($"/api/locations/{zipCode}");
            _context.Set(response, "LocationApiResponse");
        }

        [Then(@"the result should be 200")]
        public void ThenTheResultShouldBe()
        {
            var response = _context.Get<HttpResponseMessage>("LocationApiResponse");
            Assert.Equal(200, (int)response.StatusCode);
        }
    }
}
