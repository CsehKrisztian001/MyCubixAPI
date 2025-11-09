using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AddControllerTester
{
    public class AddEndpointTests : IClassFixture<WebApplicationFactory<MyCubixAPI.Program>>
    {
        private readonly HttpClient _client;

        public AddEndpointTests(WebApplicationFactory<MyCubixAPI.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData(1, 2, 3)]
        [InlineData(123456, 654321, 777777)]
        [InlineData(long.MaxValue - 1, 1, long.MaxValue)]
        public async Task TwoPositive_ReturnsSum(long a, long b, long expected)
        {
            var resp = await _client.GetAsync($"/api/Add?number1={a}&number2={b}");
            resp.EnsureSuccessStatusCode();

            var value = await resp.Content.ReadFromJsonAsync<long>();
            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData(-1, -2, -3)]
        [InlineData(-123456, -654321, -777777)]
        [InlineData(long.MinValue + 1, -1, long.MinValue)]
        public async Task TwoNegative_ReturnsSum(long a, long b, long expected)
        {
            var resp = await _client.GetAsync($"/api/Add?number1={a}&number2={b}");
            resp.EnsureSuccessStatusCode();

            var value = await resp.Content.ReadFromJsonAsync<long>();
            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData(5, -3, 2)]
        [InlineData(-10, 4, -6)]
        [InlineData(42, -42, 0)]
        public async Task MixedSigns_ReturnsSum(long a, long b, long expected)
        {
            var resp = await _client.GetAsync($"/api/Add?number1={a}&number2={b}");
            resp.EnsureSuccessStatusCode();

            var value = await resp.Content.ReadFromJsonAsync<long>();
            Assert.Equal(expected, value);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 5, 5)]
        [InlineData(-7, 0, -7)]
        public async Task OneZero_ReturnsSum(long a, long b, long expected)
        {
            var resp = await _client.GetAsync($"/api/Add?number1={a}&number2={b}");
            resp.EnsureSuccessStatusCode();

            var value = await resp.Content.ReadFromJsonAsync<long>();
            Assert.Equal(expected, value);
        }

        // Szélsőséges/túlcsordulás
        [Fact]
        public async Task Overflow_Positive_Wraps()
        {
            var resp = await _client.GetAsync($"/api/Add?number1={long.MaxValue}&number2=1");
            resp.EnsureSuccessStatusCode();

            var value = await resp.Content.ReadFromJsonAsync<long>();
            Assert.Equal(long.MinValue, value);
        }

        [Fact]
        public async Task Overflow_Negative_Wraps()
        {
            var resp = await _client.GetAsync($"/api/Add?number1={long.MinValue}&number2=-1");
            resp.EnsureSuccessStatusCode();

            var value = await resp.Content.ReadFromJsonAsync<long>();
            Assert.Equal(long.MaxValue, value);
        }

        [Theory]
        [InlineData("c", "15")]
        [InlineData("3", "ha")]
        [InlineData("g", "rt")]
        public async Task OneChar_Error(string n1, string n2)
        {
            var resp = await _client.GetAsync($"/api/Add?number1={n1}&number2={n2}");
        
            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);

            var problem = await resp.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            Assert.NotNull(problem);
            Assert.Equal(400, problem!.Status);
            Assert.Equal("One or more validation errors occurred.", problem.Title);
        }
    }
}
