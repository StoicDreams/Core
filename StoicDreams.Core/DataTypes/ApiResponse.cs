using System.Text.Json.Serialization;

namespace StoicDreams.Core.DataTypes;

public class ApiResponse
{
	public ResponseResult Result { get; set; } = ResponseResult.Success;
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public object? Data { get; set; }
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public string? Error { get; set; }
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
	public object? Debug { get; set; }
}

public class ApiResponse<TData> : ApiResponse
{
	public new TData? Data { get; set; } = default;
}
