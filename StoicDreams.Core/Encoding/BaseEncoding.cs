namespace StoicDreams.Core.Encoding;

public static class BaseEncoding
{
	private static string? BaseKeysCache;
	public static string Keys
	{
		get
		{
			BaseKeysCache ??= BuildBaseKeys;
			return BaseKeysCache;
		}
	}

	public static string BuildBaseKeys
	{
		get
		{
			StringBuilder keys = new();

			// Start with 0 to cover numbers
			int number = 48;
			while (keys.Length < 1024)
			{
				keys.Append((char)number++);
				// after numbers switch to lowercase letters
				if (number == 58) { number = 97; continue; }
				// after lowercase letters switch to uppercase letters
				if (number == 123) { number = 65; continue; }
				// after uppercase letters switch to unicode letters/characters
				if (number == 91) { number = 191; keys.Append("-_"); }
			}
			return keys.ToString();
		}
	}
}
