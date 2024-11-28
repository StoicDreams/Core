using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoicDreams.Core.Data;

public class JsonConvert : IJsonConvert
{
	public TResult<T> Deserialize<T>(string json)
	{
		TResult<T> tResult = new();
		try
		{
			tResult.Result = JsonSerializer.Deserialize<T>(json, JsonOptionsCompact);
			if (tResult.Result != null )
			{
				tResult.Status = TResultStatus.Success;
			}
		}
		catch (Exception ex)
		{
			tResult.Message = ex.Message;
		}
		return tResult;
	}

	public T Deserialize<T>(string json, Func<T> defaultIfMissing) => Deserialize<T>(json).Result ?? defaultIfMissing();

	public string Serialize(object data, bool tabbed = false)
	{
		if (tabbed) { return JsonSerializer.Serialize(data, JsonOptionsReadable); }
		return JsonSerializer.Serialize(data, JsonOptionsCompact);
	}

	public Task<TResult<T>> DeserializeAsync<T>(string json) => Task.FromResult(Deserialize<T>(json));

	public Task<T> DeserializeAsync<T>(string json, Func<T> defaultIfMissing) => Task.FromResult(Deserialize<T>(json, defaultIfMissing));

	public Task<string> SerializeAsync(object data, bool tabbed = false) => Task.FromResult(Serialize(data, tabbed));

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
