using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace ElectronicObserverTypes.Extensions;

// I have no better name right now
public static class ExtensionMethods
{
	public static bool IsKanji(this char c) => c >= 0x4E00 && c <= 0x9FBF;

	public static async Task<IEnumerable<T>> Where<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
	{
		var results = new ConcurrentQueue<T>();
		var tasks = source.Select(
			async x =>
			{
				if (await predicate(x))
					results.Enqueue(x);
			});
		await Task.WhenAll(tasks);
		return results;
	}

	public static string Display(this Enum enumValue) =>
		enumValue.GetType()
			.GetMember(enumValue.ToString())
			.First()?
			.GetCustomAttribute<DisplayAttribute>()?
			.GetName() ?? enumValue.ToString();

	// meant to be used for getting a specific language of a localized enum
	// for example when you want English EO but Japanese ship types
	// todo: not tested
	public static string Display(this Enum enumValue, CultureInfo cultureInfo)
	{
		DisplayAttribute? attribute = enumValue.GetType()
			.GetMember(enumValue.ToString())
			.First()?
			.GetCustomAttribute<DisplayAttribute>();

		if (attribute is null) return enumValue.ToString();

		Type? resourceType = attribute.ResourceType;
		string? resourceKey = attribute.Name;

		if (resourceType is null) return enumValue.ToString();
		if (resourceKey is null) return enumValue.ToString();

		PropertyInfo? resourceManagerMethodInfo = resourceType.GetProperty(nameof(ResourceManager), BindingFlags.Public | BindingFlags.Static);

		ResourceManager? resourceManager = (ResourceManager?)resourceManagerMethodInfo?.GetValue(null);

		return resourceManager?.GetString(resourceKey, cultureInfo) ?? enumValue.ToString();
	}
}
