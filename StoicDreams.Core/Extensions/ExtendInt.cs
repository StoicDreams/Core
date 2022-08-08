namespace StoicDreams.Core.Extensions;

public static class ExtendInt
{
	/// <summary>
	/// Base encode an integer to the desired base level (i.e. 8, 16, 32, 64)
	/// Supports prefix padding resulting string with 0s up to the desired string length (pad + 1)
	/// </summary>
	/// <param name="input"></param>
	/// <param name="baseValue"></param>
	/// <param name="pad">How many 0s wanted when encoded number has a single digit.</param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public static string ToBaseEncode(this int input, int baseValue = 16, int pad = 0)
	{
		if (baseValue < 2 || baseValue > BaseEncoding.Keys.Length) { throw new Exception($"Base {baseValue} is not supported for encoding."); }
		bool isNegative = input < 0;
		int currentValue = isNegative ? int.MaxValue + input : input;
		StringBuilder result = new();
		int remainder;
		do
		{
			remainder = currentValue % baseValue;
			currentValue /= baseValue;
			result.Append(BaseEncoding.Keys[remainder]);
		}
		while (currentValue > 0);
		string encoded = isNegative ? string.Join("", result.ToString().Reverse()) : string.Join("", result.ToString().Reverse());
		if (pad <= 0) { return encoded; }
		char[] padding = new char[Math.Max(0, (pad + 1) - encoded.Length)];
		Array.Fill(padding, '0');
		return $"{string.Join("", padding)}{encoded}";
	}
}
