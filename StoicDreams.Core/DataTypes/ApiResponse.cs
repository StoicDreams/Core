using System.Text.Json.Serialization;

namespace StoicDreams.Core.DataTypes;

public class ApiResponse
{
	[JsonPropertyName("result")]
	public ResponseResult Result { get; set; } = ResponseResult.Default;
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonPropertyName("data")]
	public object? Data { get; set; }
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonPropertyName("error")]
	public string? Error { get; set; }
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	[JsonPropertyName("debug")]
	public object? Debug { get; set; }
}

public class ApiResponse<TData> : ApiResponse
{
	[JsonPropertyName("data")]
	public new TData? Data { get; set; } = default;
}
