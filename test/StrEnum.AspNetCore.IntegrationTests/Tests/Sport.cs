namespace StrEnum.AspNetCore.IntegrationTests.Tests;

public class Sport : StringEnum<Sport>
{
    public static Sport TrailRunning = Define("TRAIL_RUNNING");
    public static Sport RoadCycling = Define("ROAD_CYCLING");
}