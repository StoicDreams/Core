using System.Text.Json.Serialization;

namespace StoicDreams.Core.DataTypes;

public class BaseQuery
{
	[JsonIgnore]
	/// <summary>
	/// Primary filter applied by System, not set from users.
	/// </summary>
	public string Filter { get; set; } = string.Empty;

	/// <summary>
	/// Secondary Search filter to apply based on type of query, set from users.
	/// </summary>
	public string Search { get; set; } = string.Empty;

	/// <summary>
	/// Current page number of result set
	/// </summary>
	public int Page { get; set; } = 1;

	/// <summary>
	/// Number of items per page.
	/// </summary>
	public int PerPage { get; set; } = 10;

	[JsonIgnore]
	/// <summary>
	/// Start of index for which to include items from result set.
	/// </summary>
	public int StartIndex => (Page > 0 ? Page - 1 : 0) * (PerPage > 0 ? PerPage : 10);

	[JsonIgnore]
	/// <summary>
	/// End of index for which to include items from result set.
	/// Last included index number is EndIndex - 1.
	/// </summary>
	public int EndIndex => StartIndex + (PerPage > 0 ? PerPage : 10);
}
