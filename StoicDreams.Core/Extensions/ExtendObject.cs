namespace StoicDreams.Core.Extensions;

public static class ExtendObject
{
	public static string ToJson<TInput>(this TInput input, bool tabbed = false)
	{
		if (input == null) { return string.Empty; }
		try
		{
			return new JsonConvert().Serialize(input, tabbed);
		}
		catch { return string.Empty; }
	}

	public static TResult? FromJson<TResult>(this string input)
	{
		if (input == null) { return default; }
		try
		{
			return Json.Deserialize<TResult?>(input, () => default);
		}
		catch { return default; }
	}

	public static TResult FromJson<TResult>(this string input, Func<TResult> defaultIfNull)
	{
		if (input == null) { return defaultIfNull(); }
		try
		{
			return Json.Deserialize<TResult>(input, defaultIfNull);
		}
		catch
		{
			return defaultIfNull();
		}
	}

	public static string ConvertToWebEncryptedString<TInput>(this TInput input, string? password = null)
		where TInput : new()
	{
		password ??= Config.DefaultEncryptionPassword;
		return input.ToJson().WebEncryptString(password);
	}

	public static TResult ConvertFromWebEncryptedString<TResult>(this string input, string? password = null)
		where TResult : new()
	{
		password ??= Config.DefaultEncryptionPassword;
		return input.WebDecryptString(password).FromJson<TResult>() ?? new TResult();
	}


	private static IJsonConvert Json { get; } = new JsonConvert();
}
