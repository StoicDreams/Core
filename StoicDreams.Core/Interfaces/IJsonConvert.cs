namespace StoicDreams.Core.Data;

public interface IJsonConvert
{
	T Deserialize<T>(string json, Func<T> defaultIfMissing);
	string Serialize(object data, bool tabbed = false);
	ValueTask<T> DeserializeAsync<T>(string json, Func<T> defaultIfMissing);
	ValueTask<string> SerializeAsync(object data, bool tabbed = false);
}
