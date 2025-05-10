using System.Reflection;

namespace EFCoreFiltering.Implementations.Helpers;

public static class PropertyHelper
{
    public static bool CheckHasAttribute<T>(this PropertyInfo propertyInfo) =>
        propertyInfo?.GetCustomAttributes(false).Any(x => x.GetType() == typeof(T)) ?? false;

    public static bool CheckHasAttribute<T>(this Type type) =>
        type?.GetCustomAttributes(false).Any(x => x.GetType() == typeof(T)) ?? false;
}
