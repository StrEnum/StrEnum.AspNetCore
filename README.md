# StrEnum.AspNetCore

Lets you use [StrEnum](https://github.com/StrEnum/StrEnum/) string enums with ASP.NET Core.

Supports ASP.NET Core 6 – 10. For ASP.NET Core 5, use v2.0.0.

## Installation

Install [StrEnum.AspNetCore](https://www.nuget.org/packages/StrEnum.AspNetCore/) via the .NET CLI:

```
dotnet add package StrEnum.AspNetCore
```

## Usage

### Registering the binder

If you're using `WebApplicationBuilder`, call `AddStringEnums()` in `Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddStringEnums();
```

For the ASP.NET Core 3.1–5 `IWebHostBuilder`, call `AddStringEnums()` in `ConfigureServices` in `Startup.cs`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services
        .AddControllers()
        .AddStringEnums();
}
```

### Defining a string enum and a model

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

### Binding string enums in controllers

Bind from the request body and serialize them back in the response:

```csharp
[HttpPost]
public ActionResult<Race> BodyPost([FromBody] Race race) // race.Sport is deserialized correctly
{
    return Ok(race); // race.Sport is serialized back
}
```

Bind from a route parameter:

```csharp
[HttpGet]
[Route("{sport}")]
public ActionResult<ResponseWithStrEnum> GetFromRoute(Sport sport) { ... }
```

Bind from a query string parameter:

```csharp
[HttpGet]
[Route("get")]
public ActionResult<ResponseWithStrEnum> GetFromQuery([FromQuery] Sport sport) { ... }
// `get?sport=trail_running` binds to Sport.TrailRunning
```

Bind to an array of query string parameters:

```csharp
[HttpGet]
[Route("get")]
public ActionResult<ResponseWithStrEnum> GetFromQuery([FromQuery] Sport[] sports) { ... }
// `get?sports=trail_running&sports=road_cycling` binds to [Sport.TrailRunning, Sport.RoadCycling]
```

## License

Copyright &copy; 2026 [Dmytro Khmara](https://dmytrokhmara.com).

StrEnum is licensed under the [MIT license](LICENSE.txt).
