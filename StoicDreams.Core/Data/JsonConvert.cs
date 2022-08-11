using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoicDreams.Core.Data;

public class JsonConvert : IJsonConvert
{
	public T Deserialize<T>(string json, Func<T> defaultIfMissing)
	{
		try
		{
			return JsonSerializer.Deserialize<T>(json, JsonOptionsCompact) ?? defaultIfMissing();
		}
		catch
		{
			return defaultIfMissing();
		}
	}

	public string Serialize(object data, bool tabbed = false)
	{
		if (tabbed) { return JsonSerializer.Serialize(data, JsonOptionsReadable); }
		return JsonSerializer.Serialize(data, JsonOptionsCompact);
	}

	public ValueTask<T> DeserializeAsync<T>(string json, Func<T> defaultIfMissing) => ValueTask.FromResult(Deserialize<T>(json, defaultIfMissing));

	public ValueTask<string> SerializeAsync(object data, bool tabbed = false) => ValueTask.FromResult(Serialize(data, tabbed));

	private JsonSerializerOptions JsonOptionsCompact => new()
	{
		AllowTrailingCommas = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		IncludeFields = true,
		NumberHandling = JsonNumberHandling.AllowReadingFromString,
		PropertyNameCaseInsensitive = true,
		ReadCommentHandling = JsonCommentHandling.Skip,
		WriteIndented = false,
		Encoder = JavaScriptEncoder.Default
	};

	private JsonSerializerOptions JsonOptionsReadable => new()
	{
		AllowTrailingCommas = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		IncludeFields = true,
		NumberHandling = JsonNumberHandling.AllowReadingFromString,
		PropertyNameCaseInsensitive = true,
		ReadCommentHandling = JsonCommentHandling.Skip,
		WriteIndented = true,
		Encoder = JavaScriptEncoder.Default
	};
}
