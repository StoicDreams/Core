namespace StoicDreams.Core.Data;

public interface IApiRequest
{
	ValueTask<TResult<T>> Get<T>(string url, bool isCacheable = false);
}
