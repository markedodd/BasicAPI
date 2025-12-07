using System.Reflection;

namespace BasicApp.Api.Mapping;

public static class SimpleMapper
{
    public static TDestination Map<TSource, TDestination>(TSource source)
        where TDestination : new()
    {
        if (source == null) return default!;

        var dest = new TDestination();

        var srcProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var destProps = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var dp in destProps)
        {
            if (!dp.CanWrite) continue;

            var sp = srcProps.FirstOrDefault(
                p => p.Name == dp.Name && p.PropertyType == dp.PropertyType);

            if (sp == null) continue;

            var value = sp.GetValue(source);
            dp.SetValue(dest, value);
        }

        return dest;
    }
}
