using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace StrEnum.AspNetCore.IntegrationTests.Tests
{
    public class RequestWithStrEnum
    {
        public Sport? Sport { get; set; }
    }

    public class ResponseWithStrEnum
    {
        public Sport? Sport { get; set; }
    }
    
    public class JsonPostController : Controller
    {
        [HttpPost]
        [Route("body-tests")]
        public ActionResult<ResponseWithStrEnum> Post([FromBody] RequestWithStrEnum request)
        {
            return Ok(new ResponseWithStrEnum { Sport = request.Sport });
        }
    }

    public class BodyTests
    {
        [Fact]
        public async Task Post_GivenValidStrEnumValuePassedInBodyAndReturnedInResponse_ShouldDeserializeAndSerializeStringEnumMemberValue()
        {
            var client = InMemoryApi.Run();

            var response = await client.PostAsync("/body-tests",
                new StringContent(@"{ ""sport"": ""TRAIL_RUNNING""}", Encoding.UTF8, "application/json"));

            var stringResponse = await response.Content.ReadAsStringAsync();

            stringResponse.Should().Be(@"{""sport"":""TRAIL_RUNNING""}");
        }

        [Fact]
        public async Task Post_GivenValidStrEnumNamePassedInBodyAndReturnedInResponse_ShouldDeserializeStringEnumAsNull()
        {
            var client = InMemoryApi.Run();

            var response = await client.PostAsync("/body-tests",
                new StringContent(@"{ ""sport"": ""TrailRunning""}", Encoding.UTF8, "application/json"));

            var stringResponse = await response.Content.ReadAsStringAsync();

            stringResponse.Should().Be(@"{""sport"":null}");
        }

        [Fact]
        public async Task Post_GivenInvalidStrEnumValuePassedInBody_ShouldDeserializeStringEnumAsNull()
        {
            var client = InMemoryApi.Run();

            var response = await client.PostAsync("/body-tests",
                new StringContent(@"{ ""sport"": ""QUIDDITCH""}", Encoding.UTF8, "application/json"));

            var stringResponse = await response.Content.ReadAsStringAsync();

            stringResponse.Should().Be(@"{""sport"":null}");
        }
    }
}