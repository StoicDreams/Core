namespace StoicDreams.Core.DataTypes;

/// <summary>
/// Class used to transfer data to and from storage.
/// </summary>
public class DataTransfer
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
		if (obj is DataTransfer data) { return data.ToString() == ToString(); }
		return false;
	}
}
