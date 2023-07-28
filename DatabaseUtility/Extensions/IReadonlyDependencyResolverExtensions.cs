using System;
using Splat;

namespace DatabaseUtility.Extensions;

public static class IReadonlyDependencyResolverExtensions
{
    public static T GetRequiredService<T>(this IReadonlyDependencyResolver resolver, string? contract = null)
    {
        if (resolver.GetService<T>(contract) is T obj)
            return obj;

        throw new Exception($"Unable to create required dependency of type {typeof(T).FullName}: IReadonlyDependencyResolver.GetService() returned null");
    }
}
