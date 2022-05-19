using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StrEnum.AspNetCore.ModelBinding;

internal class StringEnumModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType.IsStringEnum())
        {
            return new StringEnumModelBinder();
        }

        return null;
    }
}