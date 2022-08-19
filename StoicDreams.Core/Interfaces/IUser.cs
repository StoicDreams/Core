namespace StoicDreams.Core.Auth;

public interface IUser
{
	Guid Id { get; }
	Guid SessionId { get; }
	string Name { get; }
	int Role { get; }
}
