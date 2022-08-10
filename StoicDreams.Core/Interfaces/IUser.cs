namespace StoicDreams.Core.Interfaces;

public interface IUser
{
	Guid SessionId { get; }
	string Name { get; }
	int Role { get; }
}
