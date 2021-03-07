using Xunit;

namespace AWS.Insurance.Locations.Tests.Api.IntegrationTests.Data
{
    public class ZipCodeDataTest : TheoryData<int>
    {
        public ZipCodeDataTest()
        {
            Add(100);
            Add(2500);
            Add(5996);
            Add(7556);
            Add(9999);
        }
    }
}
