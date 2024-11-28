namespace StoicDreams.Core.Data;

public interface IJsonConvert
{
	TResult<T> Deserialize<T>(string json);
	T Deserialize<T>(string json, Func<T> defaultIfMissing);
	string Serialize(object data, bool tabbed = false);
	Task<T> DeserializeAsync<T>(string json, Func<T> defaultIfMissing);
	Task<TResult<T>> DeserializeAsync<T>(string json);
	Task<string> SerializeAsync(object data, bool tabbed = false);
}
