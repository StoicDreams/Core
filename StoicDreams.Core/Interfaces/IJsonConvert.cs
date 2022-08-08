namespace StoicDreams.Core.Data;

public interface IJsonConvert
{
	ValueTask<T> DeserializeAsync<T>(string json, Func<T> defaultIfMissing);
	ValueTask<string> SerializeAsync(object data, bool tabbed = false);
}
