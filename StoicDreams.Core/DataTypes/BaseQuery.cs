namespace StoicDreams.Core.DataTypes;

public class BaseQuery
{
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
	public int PerPage { get; set; } = 20;

	/// <summary>
	/// Start of index for which to include items from result set.
	/// </summary>
	public int StartIndex => (Page - 1) * PerPage;

	/// <summary>
	/// End of index for which to include items from result set.
	/// Last included index number is EndIndex - 1.
	/// </summary>
	public int EndIndex => StartIndex + PerPage;
}
