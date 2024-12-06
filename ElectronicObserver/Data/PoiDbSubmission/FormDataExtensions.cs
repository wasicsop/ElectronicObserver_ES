using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission;

public static class FormDataExtensions
{
	private static JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		ReferenceHandler = ReferenceHandler.IgnoreCycles
	};

	/// <param name="metaToken"></param>
	/// <param name="objectAsString">Specific for air def submission.</param>
	[return: NotNullIfNotNull(nameof(metaToken))]
	public static Dictionary<string, string>? ToKeyValue(this object? metaToken, bool objectAsString = false)
	{
		JsonElement jsonElement = JsonSerializer.SerializeToElement(metaToken, JsonSerializerOptions);

		return ToKeyValue(jsonElement, "", objectAsString);
	}

	private static Dictionary<string, string>? ToKeyValue(this JsonElement element, 
		string parentPath, bool objectAsString)
	{
		switch (element.ValueKind)
		{
			case JsonValueKind.Object when objectAsString && !string.IsNullOrEmpty(parentPath):
			{
				return new Dictionary<string, string> { { parentPath, element.ToString() } };
			}

			case JsonValueKind.Object:
			{
				Dictionary<string, string> contentData = new();
				foreach (JsonProperty property in element.EnumerateObject())
				{
					string path = parentPath switch
					{
						"" => property.Name,
						_ => $"{parentPath}[{property.Name}]",
					};

					Dictionary<string, string>? childContent = property.Value.ToKeyValue(path, objectAsString);
					if (childContent != null)
					{
						contentData = contentData
							.Concat(childContent)
							.ToDictionary(k => k.Key, v => v.Value);
					}
				}

				return contentData;
			}

			case JsonValueKind.Array:
			{
				Dictionary<string, string> contentData = new();
				foreach ((JsonElement item, int index) in element.EnumerateArray().Select((e, i) => (e, i)))
				{
					Dictionary<string, string>? childContent = item.ToKeyValue($"{parentPath}[{index}]", objectAsString);
					if (childContent != null)
					{
						contentData = contentData
							.Concat(childContent)
							.ToDictionary(k => k.Key, v => v.Value);
					}
				}

				return contentData;
			}

			case JsonValueKind.String when DateTime.TryParse(element.GetString(), out DateTime date):
			{
				string value = date.ToString("o", CultureInfo.InvariantCulture);

				return new Dictionary<string, string> { { parentPath, value } };
			}

			case JsonValueKind.String or JsonValueKind.Number or JsonValueKind.True or JsonValueKind.False:
			{
				string value = element.ToString();

				return new Dictionary<string, string> { { parentPath, value } };
			}

			default: return null;
		}
	}
}

