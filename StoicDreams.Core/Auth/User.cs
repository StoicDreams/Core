namespace StoicDreams.Core.Auth;

public class User : IUser
{
	public Guid Id { get; set; } = Guid.Empty;
	public Guid SessionId { get; set; }
	public string Name { get; set; } = "Guest";
	public int Role { get; set; }
}
