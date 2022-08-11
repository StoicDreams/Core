namespace StoicDreams.Core.Extensions;

public static class ExtendGuid
{
	public static string ToBaseEncode(this Guid input, int baseValue = 16)
	{
		byte[] bytes = input.ToByteArray();
		StringBuilder result = new();
		for (int index = 0; index < bytes.Length; index += 4)
		{
			result.Append(BitConverter.ToInt32(bytes[index..(index + 4)]).ToBaseEncode(baseValue));
		}
		return result.ToString();
	}
}
