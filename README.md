# StrEnum.AspNetCore

Allows to use [StrEnum](https://github.com/StrEnum/StrEnum/) string enums with ASP.NET Core.

Supports ASP.NET Core 6-10.

ASP.NET Core 5 supported in v2.0.0.

## Installation

You can install [StrEnum.AspNetCore](https://www.nuget.org/packages/StrEnum.AspNetCore/) using the .NET CLI:

```
dotnet add package StrEnum.AspNetCore
```

## Usage

If you're using `WebApplicationBuilder`,  add the call to `AddStringEnums()` into your `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddStringEnums();
```

If you're using the ASP.NET Core 3.1-5 `IWebHostBuilder`, call `AddStringEnums()` in the `ConfigureServices` method of your `Startup.cs`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
     services
         .AddControllers()
         .AddStringEnums();
}
```

All set. Let's now create a string enum and a model that contains it:

```csharp
public class Sport : StringEnum<Sport>
{
    public static Sport TrailRunning = Define("TRAIL_RUNNING");
    public static Sport RoadCycling = Define("ROAD_CYCLING");
}

public class Race
{
    public string Name { get; set; }
    public Sport Sport { get; set; }
}
```

You can bind string enums to the request body and return them in the response. In your controller, add the following:

```csharp
[HttpPost]
public ActionResult<Race> BodyPost([FromBody] Race race) // race.Sport is correctly deserialized
{
    return Ok(race); // race.Sport is serialized back
}
```

You can also bind string enums to a URL:

```csharp
[HttpGet]
[Route("{sport}")]
public ActionResult<ResponseWithStrEnum> GetFromRoute(Sport sport) {...}
```

To a query string parameter:

```csharp
[HttpGet]
[Route("get")]
public ActionResult<ResponseWithStrEnum> GetFromQuery([FromQuery]Sport sport) {...}
// `get?sport=trail_running` binds to Sport.TrailRunning
```

And to an array of query string parameters:

```csharp
[HttpGet]
[Route("get")]
public ActionResult<ResponseWithStrEnum> GetFromQuery([FromQuery] Sport[] sports) {...}
// `get?sports=trail_running&sports=road_cycling` binds to [Sport.TrailRunning, Sport.RoadCycling]
```

## License

Copyright &copy; 2026 [Dmytro Khmara](https://dmytrokhmara.com).

StrEnum is licensed under the [MIT license](LICENSE.txt).
