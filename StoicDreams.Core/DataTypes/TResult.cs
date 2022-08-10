using System.Diagnostics.CodeAnalysis;

namespace StoicDreams.Core.DataTypes;

public class TResult
{
	public TResultStatus Status { get; set; }
	public int StatusCode { get; set; }
	public string Message { get; set; } = string.Empty;
	public virtual bool IsOkay => Status == TResultStatus.Success;

	public static TResult Exception(string message = "Exception") => new TResult() { Message = message, Status = TResultStatus.Exception };
	public static TResult Info(string message = "Info") => new TResult() { Message = message, Status = TResultStatus.Info };
	public static TResult Success(string message = "Success") => new TResult() { Message = message, Status = TResultStatus.Success };
	public static TResult Redirect(string message = "Redirect") => new TResult() { Message = message, Status = TResultStatus.Redirect };
	public static TResult ClientError(string message = "ClientError") => new TResult() { Message = message, Status = TResultStatus.ClientError };
	public static TResult ServerError(string message = "ServerError") => new TResult() { Message = message, Status = TResultStatus.ServerError };
}

public class TResult<T> : TResult
{
	public T? Result { get; set; }

	[MemberNotNullWhen(true, "Result")]
	public override bool IsOkay => base.IsOkay && Result != null;
}
