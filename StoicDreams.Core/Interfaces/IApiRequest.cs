namespace StoicDreams.Core.Data;

public interface IApiRequest
{
	Task<TResult> GetAsync(string url, bool isCacheable = false, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);

	Task<TResult<TResponse>> GetAsync<TResponse>(string url, bool isCacheable = false, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);

	Task<TResult<TResponse>> PostJsonAsync<TResponse, TInput>(string url, TInput? postData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);

	Task<TResult> PostJsonAsync<TInput>(string url, TInput? postData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);

	Task<TResult<TResponse>> PutJsonAsync<TResponse, TInput>(string url, TInput? putData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);

	Task<TResult> PutJsonAsync<TInput>(string url, TInput? putData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);

	Task<TResult<TResponse>> DeleteAsync<TResponse>(string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);

	Task<TResult> DeleteAsync(string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default);
}
