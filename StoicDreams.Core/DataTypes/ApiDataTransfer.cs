namespace StoicDreams.Core.DataTypes;

/// <summary>
/// Class used to transfer data to and from storage.
/// </summary>
public class ApiDataTransfer
{
	/// <summary>
	/// Depending on context, could be name of class type or category.
	/// </summary>
	public string DataType { get; set; } = string.Empty;

	/// <summary>
	/// Raw data to store, encrypted and base64 encoded.
	/// </summary>
	public string Data { get; set; } = string.Empty;

	public override string ToString() => $"{DataType}:{Data}";
	public override int GetHashCode() => ToString().GetHashCode();
	public override bool Equals(object? obj)
	{
		if (obj is string text) { return text == ToString(); }
		if (obj is ApiDataTransfer data) { return data.ToString() == ToString(); }
		return false;
	}

	public static ApiDataTransfer Create(string dataType, string data) => new() { Data = data, DataType = dataType };
	public static ApiDataTransfer Create<T>(string dataType, T data, string encryptionPassword)
		where T : new()
		=> new() { Data = data.ConvertToWebEncryptedString(encryptionPassword), DataType = dataType };
}
