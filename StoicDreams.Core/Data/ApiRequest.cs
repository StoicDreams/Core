using System.Threading;

namespace StoicDreams.Core.Data;

public class ApiRequest : IApiRequest
{
	public ApiRequest(
		HttpClient httpClient,
		IJsonConvert jsonConvert
		)
	{
		Client = httpClient;
		JsonConvert = jsonConvert;
	}

	public async ValueTask<TResult<TResponse>> GetAsync<TResponse>(string url, bool isCacheable = false, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		if (isCacheable && Cache.TryGetValue(url, out object? value))
		{
			return new TResult<TResponse>()
			{
				Status = TResultStatus.Success,
				StatusCode = 200,
				Result = (TResponse)value
			};
		}
		return await SendAsync<TResponse>(new(HttpMethod.Get, url), headers);
	}

	public async ValueTask<TResult<TResponse>> PostJsonAsync<TResponse, TInput>(string url, TInput? postData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		string json = postData == null ? ""
			: postData is string jsonInput ? jsonInput
			: await JsonConvert.SerializeAsync(postData)
			;
		HttpRequestMessage request = new(HttpMethod.Post, url)
		{
			Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
		};
		return await SendAsync<TResponse>(request, headers);
	}

	public async ValueTask<TResult<TResponse>> PutJsonAsync<TResponse, TInput>(string url, TInput? putData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		string json = putData == null ? ""
			: putData is string jsonInput ? jsonInput
			: await JsonConvert.SerializeAsync(putData)
			;
		HttpRequestMessage request = new(HttpMethod.Put, url)
		{
			Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
		};
		return await SendAsync<TResponse>(request, headers);
	}

	public async ValueTask<TResult<TResponse>> DeleteAsync<TResponse>(string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
	{
		return await SendAsync<TResponse>(new(HttpMethod.Delete, url), headers);
	}

	private async ValueTask<TResult<TResponse>> SendAsync<TResponse>(HttpRequestMessage request, IDictionary<string, string>? headers, CancellationToken cancellationToken = default)
	{
		TResult<TResponse> result = new();
		try
		{
			AddHeaders(request, headers);
			HttpResponseMessage response = await Client.SendAsync(request, cancellationToken);
			return await ProcessResponse<TResponse>(response);
		}
		catch (Exception ex)
		{
			result.Message = ex.Message;
		}
		return result;
	}

	private void AddHeaders(HttpRequestMessage request, IDictionary<string, string>? headers)
	{
		if (headers == null || headers.Count == 0) { return; }
		foreach (string tag in headers.Keys)
		{
			request.Headers.TryAddWithoutValidation(tag, headers[tag]);
		}
	}

	private async ValueTask<TResult<TResponse>> ProcessResponse<TResponse>(HttpResponseMessage response)
	{
		TResult<TResponse> result = new();
		result.StatusCode = (int)response.StatusCode;
		result.Status = result.StatusCode switch
		{
			< 100 => TResultStatus.Exception,
			>= 100 and < 200 => TResultStatus.Info,
			>= 200 and < 300 => TResultStatus.Success,
			>= 300 and < 400 => TResultStatus.Redirect,
			>= 400 and < 500 => TResultStatus.ClientError,
			_ => TResultStatus.ServerError
		};
		string json = await response.Content.ReadAsStringAsync();
		if (TryDeserializeApiResponse(json, out TResult<TResponse>? apiResponse) && apiResponse != null)
		{
			return apiResponse;
		}
		else if (json is TResponse data)
		{
			result.Result = data;
		}
		else if (TryDeserializeExpected(json, out TResponse? expectedResponse))
		{
			result.Result = expectedResponse;
		}
		if (result.Status == TResultStatus.Success && result.Result == null)
		{
			result.Status = TResultStatus.Exception;
			result.Message = "Incoming data did not match expected format.";
		}
		return result;
	}

	private bool TryDeserializeExpected<TResponse>(string json, out TResponse? response)
	{
		response = default;
		try
		{
			response = System.Text.Json.JsonSerializer.Deserialize<TResponse>(json);
			return true;
		}
		catch
		{
			return false;
		}
	}

	private bool TryDeserializeApiResponse<TResponse>(string json, out TResult<TResponse>? response)
	{
		response = default;
		try
		{
			ApiResponse<TResponse>? apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<TResponse>>(json);
			if (apiResponse == null) { return false; }
			if (apiResponse.Data is not TResponse) { return false; }
			response = apiResponse;
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static Dictionary<string, object> Cache { get; } = new();

	private HttpClient Client { get; }
	private IJsonConvert JsonConvert { get; }
}
