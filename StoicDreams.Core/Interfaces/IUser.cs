﻿namespace StoicDreams.Core.Auth;

public interface IUser
{
	Guid SessionId { get; }
	string Name { get; }
	int Role { get; }
}
