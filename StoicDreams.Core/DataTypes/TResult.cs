﻿using System.Diagnostics.CodeAnalysis;

namespace StoicDreams.Core.DataTypes;

public struct TResult
{
	public TResult()
	{

	}

	internal object? Result { get; set; }
	internal bool ExpectResult { get; set; }
	public TResultStatus Status { get; set; }
	public int StatusCode { get; set; }
	public string Message { get; set; } = string.Empty;
	public bool IsOkay => Status == TResultStatus.Success && (!ExpectResult | Result != null);

	public override string ToString()
	{
		return $"{IsOkay}_{StatusCode}_{Status}_{Message}";
	}

	public override bool Equals(object? obj)
	{
		if (obj == null) { return false; }
		if (obj is not TResult instance) { return false; }
		return ToString() == instance.ToString();
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	public static TResult Exception(string message = "Exception") => new() { Message = message, Status = TResultStatus.Exception };
	public static TResult Info(string message = "Info") => new() { Message = message, Status = TResultStatus.Info };
	public static TResult Success(string message = "Success") => new() { Message = message, Status = TResultStatus.Success };
	public static TResult Redirect(string message = "Redirect") => new() { Message = message, Status = TResultStatus.Redirect };
	public static TResult ClientError(string message = "ClientError") => new() { Message = message, Status = TResultStatus.ClientError };
	public static TResult ServerError(string message = "ServerError") => new() { Message = message, Status = TResultStatus.ServerError };

	public static implicit operator TResult(ApiResponse response)
	{
		TResult result = new()
		{
			Message = response.Error ?? string.Empty,
			Status = response.Result switch
			{
				ResponseResult.Success => TResultStatus.Success,
				ResponseResult.RedirectInfo => TResultStatus.Redirect,
				ResponseResult.HardRedirect => TResultStatus.Redirect,
				_ => TResultStatus.Exception
			}
		};
		if (response.Data is string message)
		{
			result.Message = message.CleanMessage();
		}
		return result;
	}
}

public struct TResult<T>
{
	public TResult()
	{
	}

	public T? Result { get; set; }
	public TResultStatus Status { get; set; }
	public int StatusCode { get; set; }
	public string Message { get; set; } = string.Empty;
	[MemberNotNullWhen(true, "Result")]
	public bool IsOkay => Status == TResultStatus.Success && Result != null;

	public override string ToString()
	{
		return $"{IsOkay}_{StatusCode}_{Status}_{Result}{Message}";
	}

	public override bool Equals(object? obj)
	{
		if (obj == null) { return false; }
		if (obj is not TResult<T> instance) { return false; }
		return ToString() == instance.ToString();
	}

	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	public static implicit operator TResult(TResult<T> input)
	{
		return new()
		{
			Result = input.Result,
			ExpectResult = true,
			Status = input.Status,
			StatusCode = input.StatusCode,
			Message = input.Message,
		};
	}

	public static implicit operator TResult<T>(ApiResponse response)
	{
		TResult<T> result = new()
		{
			Message = response.Error ?? string.Empty,
			Status = response.Result switch
			{
				ResponseResult.Success => TResultStatus.Success,
				ResponseResult.RedirectInfo => TResultStatus.Redirect,
				ResponseResult.HardRedirect => TResultStatus.Redirect,
				_ => TResultStatus.Exception
			}
		};
		if (!(response.Data is T data))
		{
			result.Status = TResultStatus.Exception;
			if (result.Message == string.Empty) { result.Message = "An unexpected error occurred."; }
			return result;
		}
		result.Result = data;
		if (data is string message)
		{
			result.Message = message.CleanMessage();
			if (result.Message is T cleanedResult)
			{
				result.Result = cleanedResult;
			}
		}
		return result;
	}

	public static implicit operator TResult<T>(ApiResponse<T> response)
	{
		TResult<T> result = new()
		{
			Message = response.Error ?? string.Empty,
			Status = response.Result switch
			{
				ResponseResult.Success => TResultStatus.Success,
				ResponseResult.RedirectInfo => TResultStatus.Redirect,
				ResponseResult.HardRedirect => TResultStatus.Redirect,
				_ => TResultStatus.Exception
			}
		};
		if (response.Data is string message)
		{
			result.Message = message;
		}
		if (response.Data is not T data)
		{
			result.Status = TResultStatus.Exception;
			if (result.Message == string.Empty) { result.Message = "An unexpected error occurred."; }
			return result;
		}
		result.Result = data;
		return result;
	}

	public static TResult<T> Exception(string message = "Exception") => new() { Message = message, Status = TResultStatus.Exception };
	public static TResult<T> Info(string message = "Info") => new() { Message = message, Status = TResultStatus.Info };
	public static TResult<T> Success(T item, string message = "Success") => new() { Message = message, Status = TResultStatus.Success, Result = item };
	public static TResult<T> Redirect(string message = "Redirect") => new() { Message = message, Status = TResultStatus.Redirect };
	public static TResult<T> ClientError(string message = "ClientError") => new() { Message = message, Status = TResultStatus.ClientError };
	public static TResult<T> ServerError(string message = "ServerError") => new() { Message = message, Status = TResultStatus.ServerError };
}
