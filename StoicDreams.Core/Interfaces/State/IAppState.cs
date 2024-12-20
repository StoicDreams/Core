﻿namespace StoicDreams.Core.State;

public interface IAppState : IStateManager
{
	/// <summary>
	/// Set data for the specified data tag.
	/// Note: SetData does not fire event triggers.
	/// Wrap SetData calles in ApplyChanges method or call TriggerChange after last SetData is called.
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	/// <param name="tag"></param>
	/// <param name="data"></param>
	void SetData<TData>(AppStateDataTags tag, TData? data);

	/// <summary>
	/// Get data for the specified data tag.
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	/// <param name="tag"></param>
	/// <returns></returns>
	TData? GetData<TData>(AppStateDataTags tag);

	/// <summary>
	/// Apply a group of updates and fire change trigger after updates.
	/// </summary>
	/// <param name="changeHandler"></param>
	void ApplyChanges(Action changeHandler);

	/// <summary>
	/// Trigger state change for 1 or more app state tags.
	/// </summary>
	/// <param name="tags"></param>
	/// <returns></returns>
	Task TriggerChangeAsync(params AppStateDataTags[] tags);
}
