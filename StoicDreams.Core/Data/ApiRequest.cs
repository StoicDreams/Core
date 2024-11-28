using System.Diagnostics.CodeAnalysis;

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

    public async Task<TResult> GetAsync(string url, bool isCacheable = false, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        if (isCacheable && Cache.TryGetValue(url, out object? value))
        {
            return new TResult()
            {
                Status = TResultStatus.Success,
                StatusCode = 200,
                Message = value?.ToString() ?? string.Empty
            };
        }
        TResult result = await SendAsync(new(HttpMethod.Get, url), headers, cancellationToken);
        if (result.IsOkay && isCacheable)
        {
            Cache[url] = result.Message;
        }
        return result;
    }

    public async Task<TResult<TResponse>> GetAsync<TResponse>(string url, bool isCacheable = false, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
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
        TResult<TResponse> tResult = await SendAsync<TResponse>(new(HttpMethod.Get, url), headers, cancellationToken);
        if (tResult.IsOkay && isCacheable)
        {
            Cache[url] = tResult.Result;
        }
        return tResult;
    }

    public async Task<TResult<TResponse>> PostJsonAsync<TResponse, TInput>(string url, TInput? postData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        string json = postData == null ? ""
            : postData is string jsonInput ? jsonInput
            : await JsonConvert.SerializeAsync(postData)
            ;
        HttpRequestMessage request = new(HttpMethod.Post, url)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };
        return await SendAsync<TResponse>(request, headers, cancellationToken);
    }

    public async Task<TResult> PostJsonAsync<TInput>(string url, TInput? postData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        string json = postData == null ? ""
            : postData is string jsonInput ? jsonInput
            : await JsonConvert.SerializeAsync(postData)
            ;
        HttpRequestMessage request = new(HttpMethod.Post, url)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };
        return await SendAsync(request, headers, cancellationToken);
    }

    public async Task<TResult<TResponse>> PutJsonAsync<TResponse, TInput>(string url, TInput? putData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        string json = putData == null ? ""
            : putData is string jsonInput ? jsonInput
            : await JsonConvert.SerializeAsync(putData)
            ;
        HttpRequestMessage request = new(HttpMethod.Put, url)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };
        return await SendAsync<TResponse>(request, headers, cancellationToken);
    }

    public async Task<TResult> PutJsonAsync<TInput>(string url, TInput? putData = default, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        string json = putData == null ? ""
            : putData is string jsonInput ? jsonInput
            : await JsonConvert.SerializeAsync(putData)
            ;
        HttpRequestMessage request = new(HttpMethod.Put, url)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        };
        return await SendAsync(request, headers);
    }

    public async Task<TResult<TResponse>> DeleteAsync<TResponse>(string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        return await SendAsync<TResponse>(new(HttpMethod.Delete, url), headers, cancellationToken);
    }

    public async Task<TResult> DeleteAsync(string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
    {
        return await SendAsync(new(HttpMethod.Delete, url), headers, cancellationToken);
    }

    private async Task<TResult<TResponse>> SendAsync<TResponse>(HttpRequestMessage request, IDictionary<string, string>? headers, CancellationToken cancellationToken = default)
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

    private async Task<TResult> SendAsync(HttpRequestMessage request, IDictionary<string, string>? headers, CancellationToken cancellationToken = default)
    {
        TResult result = new();
        try
        {
            AddHeaders(request, headers);
            HttpResponseMessage response = await Client.SendAsync(request, cancellationToken);
            return await ProcessResponse(response);
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

    private async Task<TResult<TResponse>> ProcessResponse<TResponse>(HttpResponseMessage response)
    {
        TResult<TResponse> result = new()
        {
            StatusCode = (int)response.StatusCode
        };
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
        if (TryDeserializeApiResponse(json, out TResult<TResponse> apiResponse))
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

    private async Task<TResult> ProcessResponse(HttpResponseMessage response)
    {
        TResult result = new()
        {
            StatusCode = (int)response.StatusCode
        };
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
        if (TryDeserializeApiResponse(json, out TResult apiResponse))
        {
            return apiResponse;
        }
        result.Message = json;
        return result;
    }

    private bool TryDeserializeExpected<TResponse>(string json, out TResponse? response)
    {
        response = default;
        try
        {
            TResult<TResponse> tResult = JsonConvert.Deserialize<TResponse>(json);
            response = tResult.Result;
            return tResult.IsOkay;
        }
        catch
        {
            return false;
        }
    }

    private bool TryDeserializeApiResponse<TResponse>(string json, [NotNullWhen(true)] out TResult<TResponse> response)
    {
        response = default;
        try
        {
            {
                TResult<ApiResponse<TResponse>> tResult = JsonConvert.Deserialize<ApiResponse<TResponse>>(json);
                ApiResponse<TResponse>? apiResponse = tResult.Result;
                if (tResult.IsOkay && apiResponse != null && apiResponse.Result != ResponseResult.Default)
                {
                    response = apiResponse;
                    return true;
                }
            }
            {
                TResult<ApiResponse> tResult = JsonConvert.Deserialize<ApiResponse>(json);
                ApiResponse? apiResponse = tResult.Result;
                if (tResult.IsOkay && apiResponse != null && apiResponse.Result != ResponseResult.Default)
                {
                    response = apiResponse;
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            return false;
        }
    }

    private bool TryDeserializeApiResponse(string json, [NotNullWhen(true)] out TResult response)
    {
        response = default;
        try
        {
            {
                TResult<ApiResponse> tResult = JsonConvert.Deserialize<ApiResponse>(json);
                ApiResponse? apiResponse = tResult.Result;
                if (tResult.IsOkay && apiResponse != null && apiResponse.Result != ResponseResult.Default)
                {
                    response = apiResponse;
                    return true;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            string error = ex.Message;
            return false;
        }
    }

    private static Dictionary<string, object> Cache { get; } = [];

    private HttpClient Client { get; }
    private IJsonConvert JsonConvert { get; }
}
