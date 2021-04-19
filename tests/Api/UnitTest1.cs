using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Tutorial.Api.Tests
{
    public class UnitTest1
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public UnitTest1()
        {
            _factory = new WebApplicationFactory<Startup>();
        }
        
        [Fact]
        public async Task Test1()
        {
            using var client = _factory.CreateClient();
            
            var response = await client.GetAsync("/weatherforecast");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
