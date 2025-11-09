using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AddControllerTester
{
    public class AddEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AddEndpointTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(123456, 654321, 777777)]
        [InlineData(long.MaxValue - 1, 1, long.MaxValue)]
        public async Task TwoPositive_ReturnsSum(long a, long b, long expected)
        {
            var resp = await _client.GetAsync($"/Add?number1={a}&number2={b}");
            resp.EnsureSuccessStatusCode();

            var value = await resp.Content.ReadFromJsonAsync<long>();
            Assert.Equal(expected, value);
        }
    }
}
