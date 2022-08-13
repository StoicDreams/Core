namespace StoicDreams.Core.Data;

public interface IApiRequest
{
	ValueTask<TResult<TResponse>> GetAsync<TResponse>(string url, bool isCacheable = false, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
	ValueTask<TResult<TResponse>> PostJsonAsync<TResponse, TInput>(string url, TInput? postData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
	ValueTask<TResult<TResponse>> PutJsonAsync<TResponse, TInput>(string url, TInput? putData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
	ValueTask<TResult<TResponse>> DeleteAsync<TResponse>(string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
}
