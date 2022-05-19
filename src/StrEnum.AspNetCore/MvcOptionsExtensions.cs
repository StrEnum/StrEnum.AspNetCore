using Microsoft.Extensions.DependencyInjection;
using StrEnum.AspNetCore.ModelBinding;
using StrEnum.System.Text.Json;
using StrEnum.System.Text.Json.Converters;

namespace StrEnum.AspNetCore;

public static class MvcBuilderExtensions
{
    /// <summary>
    /// Enables ASP.NET Core to bind string enums from requests and serialize them into responses.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IMvcBuilder AddStringEnums(this IMvcBuilder builder)
    {
        builder.AddMvcOptions(o => o.ModelBinderProviders.Insert(0, new StringEnumModelBinderProvider()));

        builder.AddJsonOptions(o => o.JsonSerializerOptions.UseStringEnums(NoMemberFoundBehavior.ReturnNull));

        return builder;
    }
}