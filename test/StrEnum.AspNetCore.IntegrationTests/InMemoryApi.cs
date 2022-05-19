using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace StrEnum.AspNetCore.IntegrationTests;

public class InMemoryApi
{
    public static HttpClient Run()
    {
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices((services) =>
                {
                    services
                        .AddControllers()
                        .AddStringEnums();
                });
                builder.UseSolutionRelativeContentRoot("test");
            });

        var client = application.CreateClient();
        return client;
    }
}