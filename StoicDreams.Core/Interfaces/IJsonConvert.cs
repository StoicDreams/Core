﻿namespace StoicDreams.Core.Data;

public interface IJsonConvert
{
	TResult<T> Deserialize<T>(string json);
	T Deserialize<T>(string json, Func<T> defaultIfMissing);
	string Serialize(object data, bool tabbed = false);
	ValueTask<T> DeserializeAsync<T>(string json, Func<T> defaultIfMissing);
	ValueTask<TResult<T>> DeserializeAsync<T>(string json);
	ValueTask<string> SerializeAsync(object data, bool tabbed = false);
}
