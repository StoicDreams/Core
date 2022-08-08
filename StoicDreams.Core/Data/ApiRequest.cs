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

	public async ValueTask<TResult<TResponse>> GetAsync<TResponse>(string url, bool isCacheable = false)
	{
		TResult<TResponse> result = new();
		try
		{
			if (isCacheable && Cache.TryGetValue(url, out object? value))
			{
				result.Status = TResultStatus.Success;
				result.StatusCode = 200;
				result.Result = (TResponse)value;
				return result;
			}
			HttpResponseMessage response = await Client.GetAsync(url);
			return await ProcessResponse<TResponse>(response);
		}
		catch (Exception ex)
		{
			result.Message = ex.Message;
		}
		return result;
	}

	public async ValueTask<TResult<TResponse>> PostJsonAsync<TResponse, TInput>(string url, TInput? postData = default)
	{
		try
		{
			string json = postData == null ? ""
				: postData is string jsonInput ? jsonInput
				: await JsonConvert.SerializeAsync(postData)
				;
			StringContent content = new (json, System.Text.Encoding.UTF8, "application/json");
			HttpResponseMessage response = await Client.PostAsync(url, content);
			return await ProcessResponse<TResponse>(response);
		}
		catch (Exception ex)
		{
			return new TResult<TResponse>()
			{
				Message = ex.Message
			};
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
		if (result.Status == TResultStatus.Success && result.Result == null)
		{
			result.Status = TResultStatus.Exception;
			result.Message = "Incoming data did not match expected format.";
		}
		if (json is TResponse data)
		{
			result.Result = data;
		}
		else
		{
			result.Result = System.Text.Json.JsonSerializer.Deserialize<TResponse>(json);
		}
		return result;
	}

	private static Dictionary<string, object> Cache { get; } = new();

	private HttpClient Client { get; }
	private IJsonConvert JsonConvert { get; }
}
