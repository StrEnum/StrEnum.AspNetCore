using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace StrEnum.AspNetCore.IntegrationTests.Tests
{

    public class UrlController : Controller
    {
        [HttpGet]
        [Route("url-tests/from-route/{sport}")]
        public ActionResult<ResponseWithStrEnum> GetFromRoute(Sport sport)
        {
            return Ok(sport);
        }

        [HttpGet]
        [Route("url-tests/from-query")]
        public ActionResult<ResponseWithStrEnum> GetFromQuery([FromQuery]Sport sport)
        {
            return Ok(sport);
        }

        [HttpGet]
        [Route("url-tests/from-query-array")]
        public ActionResult<ResponseWithStrEnum> GetFromQuery([FromQuery] Sport[] sports)
        {
            return Ok(sports);
        }
    }

    public class UrlTests
    {
        [Fact]
        public async Task Get_GivenStrEnumMemberValueInRequestUrl_ShouldParseStringEnumMemberFromUrl()
        {
            var client = InMemoryApi.Run();

            var response = await client.GetAsync("/url-tests/from-route/trail_running");

            var stringResponse = await response.Content.ReadAsStringAsync();

            stringResponse.Should().Be("\"TRAIL_RUNNING\"");
        }

        [Fact]
        public async Task Get_GivenStrEnumMemberNameInRequestUrl_ShouldParseStringEnumAsNull()
        {
            var client = InMemoryApi.Run();

            var response = await client.GetAsync("/url-tests/from-route/trailrunning");

            var stringResponse = await response.Content.ReadAsStringAsync();

            stringResponse.Should().Be("");
        }

        [Fact]
        public async Task Get_GivenStrEnumValueIsInQueryString_ShouldParseStringEnumMember()
        {
            var client = InMemoryApi.Run();

            var response = await client.GetAsync("/url-tests/from-query?sport=trail_running");

            var stringResponse = await response.Content.ReadAsStringAsync();

            stringResponse.Should().Be("\"TRAIL_RUNNING\"");
        }

        [Fact]
        public async Task Get_GivenMultipleStrEnumValuesInQueryString_ShouldParseStringEnumMember()
        {
            var client = InMemoryApi.Run();

            var response = await client.GetAsync("/url-tests/from-query-array?sports=trail_running&sports=road_cycling");

            var stringResponse = await response.Content.ReadAsStringAsync();

            stringResponse.Should().Be("[\"TRAIL_RUNNING\",\"ROAD_CYCLING\"]");
        }
    }
}