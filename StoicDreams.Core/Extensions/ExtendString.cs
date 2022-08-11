using System.Net;
using System.Security.Cryptography;

namespace StoicDreams.Core.Extensions;

public static class ExtendString
{
	/// <summary>
	/// Return value with Asterix characters substituted for most of the characters.
	/// Shorter text substitutes all characters but the first.
	/// Longer text will retain first 3 and last 3 characters, and display 3 asterix characters inbetween.
	/// </summary>
	/// <param name="value"></param>
	/// <param name="shortCutoff">Must be at least 5</param>
	/// <returns></returns>
	public static string FilterValue(this string value, int shortCutoff = 7)
	{
		if (string.IsNullOrWhiteSpace(value)) { return string.Empty; }
		if (shortCutoff < 5) { shortCutoff = 5; }
		if (value.Length < shortCutoff)
		{
			char[] result = new char[value.Length];
			Array.Fill(result, '*');
			result[0] = value[0];
			return string.Join("", result);
		}
		return $"{value[0..3]}***{value[^3..]}";
	}

	/// <summary>
	/// Parse the input text for the equivelent T enum value based on either text or numeric matching.
	/// Returns default value if parsing fails.
	/// Parsing is case insensitive.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="input"></param>
	/// <returns></returns>
	public static T AsEnum<T>(this string input)
		where T : struct
	{
		return Enum.TryParse(typeof(T), input, true, out object? value) && value is T success ? success : default;
	}

	/// <summary>
	/// Transform json input from minimized to double-space-tabbed json.
	/// </summary>
	/// <param name="json"></param>
	/// <returns></returns>
	public static string PrettifyJson(this string json)
	{
		try
		{
			IJsonConvert converter = new JsonConvert();
			object? data = converter.Deserialize<object>(json, () => new());
			if (data == null) { return json; }
			return converter.Serialize(data, tabbed: true);
		}
		catch
		{
			return json;
		}
	}

	/// <summary>
	/// Produe a hash code for the given string that will remain constant across implementations and app restarts.
	/// </summary>
	/// <param name="str"></param>
	/// <returns></returns>
	public static int GetStaticHashCode(this string str)
	{
		if (string.IsNullOrEmpty(str)) { return 0; }
		unchecked
		{
			int hash1 = 5381;
			int hash2 = hash1;

			for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
			{
				hash1 = ((hash1 << 5) + hash1) ^ str[i];
				if (i == str.Length - 1 || str[i + 1] == '\0')
					break;
				hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
			}

			return hash1 + (hash2 * 1566083941);
		}
	}

	/// <summary>
	/// Get the parent folder of the provided path if any.
	/// Returns path if path is the root folder.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string GetParentFolder(this string path)
	{
		DirectoryInfo dir = new(path);
		return dir?.Parent?.FullName ?? dir?.FullName ?? path;
	}

	/// <summary>
	/// Get the first parent folder of the top-most folder that matches the given childFolder name.
	/// Matching is case-insensitive.
	/// Returns given path if childFolder is not found.
	/// </summary>
	/// <param name="path"></param>
	/// <param name="childFolder"></param>
	/// <param name="stopFirstMatch">Set true to return parent of first folder matching childFolder name instead of last/top-most</param>
	/// <returns></returns>
	public static string BubbleUpToParentIfChildFolderExists(this string path, string childFolder, bool stopFirstMatch = false)
	{
		DirectoryInfo dir = new(path);
		DirectoryInfo parent = dir;
		while (dir.Parent != null)
		{
			if (dir.Name.ToLower() == childFolder.ToLower())
			{
				if (stopFirstMatch) { return dir.Parent.FullName; }
				parent = dir.Parent;
			}
			dir = dir.Parent;
		}
		return parent.FullName;
	}

	/// <summary>
	/// HTML encode string to assure no HTML injection is applied.
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string HtmlEncode(this string input)
	{
		return WebUtility.HtmlEncode(input);
	}

	/// <summary>
	/// Transform a base64 encoded string to be url safe.
	/// Trims '=' characters and replaces '+' with '_' and '/' with '-'.
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string Base64ToUrlSafe(this string input)
	{
		return input
			.Replace("=", "")
			.Replace("+", "_")
			.Replace("/", "-")
			;
	}

	/// <summary>
	/// Transform a base64 string previously made url safe back to base64 native format.
	/// Adds '=' until length of string is divisible by 4 and replaces '_' with '+' and '-' with '/';
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string Base64FromUrlSafe(this string input)
	{
		while (input.Length % 4 != 0)
		{
			input += "=";
		}
		return input.Replace("_", "+").Replace("-", "/");
	}

	/// <summary>
	/// Convert string to Base64 encoded string with trimmed off '=' characters.
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string ToBase64(this string input, bool makeUrlSafe = false)
	{
		if (string.IsNullOrWhiteSpace(input)) { return ""; }
		string base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input.Trim()));
		if (!makeUrlSafe) { return base64; }
		return base64.Base64ToUrlSafe();
	}

	/// <summary>
	/// Convert Base64 encoded string to decoded string.
	/// Will autofill missing '=' characters to Base64 before converting.
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string FromBase64(this string input)
	{
		if (string.IsNullOrWhiteSpace(input)) { return ""; }
		return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(input.Base64FromUrlSafe()));
	}

	/// <summary>
	/// Apply a very basic encryption to a string.
	/// This is a web-browser runable encryption method when using Blazor to build client UIs.
	/// </summary>
	/// <param name="input"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	public static string WebEncryptString(this string input, string password)
	{
		if (string.IsNullOrWhiteSpace(input)) { return string.Empty; }
		byte[] data = System.Text.Encoding.UTF8.GetBytes(input);
		byte hash = (byte)password.GetStaticHashCode();
		for (int index = 0; index < data.Length; ++index)
		{
			data[index] = (byte)(data[index] + hash);
		}
		return Convert.ToBase64String(data).Base64ToUrlSafe();
	}

	/// <summary>
	/// Decrypt a web encrypted string (encrypted by WebEncryptString).
	/// This is a web-browser runable decryption method when using Blazor to build client UIs.
	/// </summary>
	/// <param name="input"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	public static string WebDecryptString(this string input, string password)
	{
		if (string.IsNullOrWhiteSpace(input)) { return string.Empty; }
		byte hash = (byte)password.GetStaticHashCode();
		byte[] data = Convert.FromBase64String(input.Base64FromUrlSafe());
		for (int index = 0; index < data.Length; ++index)
		{
			data[index] = (byte)(data[index] - hash);
		}
		return System.Text.Encoding.UTF8.GetString(data);
	}

	/// <summary>
	/// Compute a string hash for the given string.
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string Hash(this string input)
	{
		using SHA512 encryptor = SHA512.Create();
		byte[] bytes = encryptor.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
		return Convert.ToBase64String(bytes).Base64ToUrlSafe();
	}

	/// <summary>
	/// Convert a Pascal formatted string (e.g. SomeText) into a space delimited string with separations at capitalized letters (e.g. Some Text).
	/// </summary>
	/// <param name="input"></param>
	/// <returns></returns>
	public static string PascalToSpaced(this string input)
	{
		if (string.IsNullOrWhiteSpace(input)) { return string.Empty; }
		StringBuilder result = new();
		char last = ' ';
		foreach (char c in input)
		{
			if (char.IsUpper(c) && last != ' ') { result.Append(' '); }
			result.Append(c);
			last = c;
		}
		return result.ToString();
	}
}
