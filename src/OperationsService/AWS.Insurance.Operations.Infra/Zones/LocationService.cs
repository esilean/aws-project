using AWS.Insurance.Operations.Application.Customers.Dtos;
using AWS.Insurance.Operations.Application.Errors;
using AWS.Insurance.Operations.Application.Gateways;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Infra.Zones
{
    public class LocationService : ILocationService
    {
        private readonly AsyncRetryPolicy<HttpResponseMessage> TransientErrorRetryPolicy;
        private static readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> CircuitBreakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

        private readonly ILogger<LocationService> _logger;
        private readonly HttpClient _client;

        public LocationService(ILogger<LocationService> logger, IHttpClientFactory factory)
        {
            _logger = logger;

            _client = factory.CreateClient("LocationService");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            TransientErrorRetryPolicy = Policy
                .HandleResult<HttpResponseMessage>
                (message => ((int)message.StatusCode == 400 || (int)message.StatusCode >= 500))
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    _logger.LogWarning(1, "Trying to request again... RetryAttempt {retryAttempt}", retryAttempt);
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                });

        }

        public async Task<LocationDto> GetLocation(int zipCode)
        {

            if (CircuitBreakerPolicy.CircuitState == CircuitState.Open)
                throw new BrokenCircuitException("Locations API is down!");

            var response = await CircuitBreakerPolicy
                                                .ExecuteAsync(() =>
                                                    TransientErrorRetryPolicy
                                                    .ExecuteAsync(() => _client.GetAsync($"locations/{zipCode}")));
            if (!response.IsSuccessStatusCode)
                throw new RestException(response.StatusCode, new { message = "Error retrieving zone from Location Api" });

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<LocationDto>(content);
        }

    }
}