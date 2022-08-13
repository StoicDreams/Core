namespace StoicDreams.Core.Auth;

public interface IClientAuth
{
	/// <summary>
	/// Basic user information needed by system.
	/// </summary>
	IUser User { get; set; }

	/// <summary>
	/// Current state if user is logged in (true) or not (false).
	/// </summary>
	bool IsLoggedIn { get; }

	/// <summary>
	/// Current roles assigned to user.
	/// 0 is reserved for "Guest" role and signifies they are not logged in.
	/// </summary>
	/// <param name="role"></param>
	/// <returns></returns>
	bool IsRole(int role);

	/// <summary>
	/// Validation check to verify if email meets expected format and other requirements if any.
	/// </summary>
	/// <param name="email"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	bool EmailIsValid(string email, out string message);

	/// <summary>
	/// Validation check to verify if password meets minimum requirements.
	/// </summary>
	/// <param name="password"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	bool PasswordIsValid(string password, out string message);

	/// <summary>
	/// Process typically used during startup to check if the user is currently logged in from a previous session.
	/// </summary>
	/// <returns></returns>
	ValueTask CheckLoginFromCache();

	/// <summary>
	/// Processing to handle signing in an existing user.
	/// </summary>
	/// <param name="email"></param>
	/// <param name="password"></param>
	/// <returns></returns>
	ValueTask<TResult> SignIn(string email, string password);

	/// <summary>
	/// Processing to handle signing up a new user.
	/// </summary>
	/// <param name="email"></param>
	/// <param name="displayName"></param>
	/// <returns></returns>
	ValueTask<TResult> SignUp(string email, string displayName);

	/// <summary>
	/// Processing to sign out a currently logged in user.
	/// </summary>
	/// <returns></returns>
	ValueTask<TResult> LogOut();

	/// <summary>
	/// Processing to update password.
	/// </summary>
	/// <returns></returns>
	ValueTask<TResult> UpdatePassword(string password);
}
