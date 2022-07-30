using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrEnum.AspNetCore.ModelBinding;

internal class StringEnumModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value))
        {
            return Task.CompletedTask;
        }

        var parsed = TryParseStringEnum(bindingContext.ModelType, value, out var parsedMember);

        if (parsed)
        {
            bindingContext.Result = ModelBindingResult.Success(parsedMember);

            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }

    private static bool TryParseStringEnum(Type modelType, string value, out object? member)
    {
        var stringEnumBase = typeof(StringEnum<>).MakeGenericType(modelType);

        var tryParseMethod = stringEnumBase.GetMethod("TryParse",
            new[] { typeof(string), modelType.MakeByRefType(), typeof(bool), typeof(MatchBy) });

        var tryParseParameters = new object?[] { value, null, true, MatchBy.ValueOnly };

        var parsed = (bool)(tryParseMethod?.Invoke(null, tryParseParameters) ?? false);

        if (parsed)
        {
            member = tryParseParameters[1];

            return true;
        }

        member = null;

        return false;
    }
}