namespace StoicDreams.Core.Data;

public interface IApiRequest
{
	ValueTask<TResult<TResponse>> GetAsync<TResponse>(string url, bool isCacheable = false);
	ValueTask<TResult<TResponse>> PostJsonAsync<TResponse, TInput>(string url, TInput? postData = default);
}
