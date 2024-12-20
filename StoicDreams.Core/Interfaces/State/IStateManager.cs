﻿namespace StoicDreams.Core.State;

public interface IStateManager
{
	/// <summary>
	/// Check if state manager currently contains the requested state.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	Task<bool> ContainsState(string name);

	/// <summary>
	/// Try to get state based on requested key name and type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="name"></param>
	/// <returns></returns>
	Task<TResult<T>> TryGetState<T>(string name);

	/// <summary>
	/// Set data for the specified custom tag name.
	/// Note: SetData does not fire event triggers.
	/// Wrap SetData calles in ApplyChanges method or call TriggerChange after last SetData is called.
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	/// <param name="name"></param>
	/// <param name="data"></param>
	Task SetDataAsync<TData>(string name, TData? data);

	/// <summary>
	/// Get data for the specified custom tag name
	/// </summary>
	/// <typeparam name="TData"></typeparam>
	/// <param name="name"></param>
	/// <returns></returns>
	Task<TData?> GetDataAsync<TData>(string name);

	/// <summary>
	/// Subscribe to event trigger called when a state has changed.
	/// When using this make component IDisposable and make sure to call UnsubscribeToDataChanges on Dispose to properly cleanup subscriptions.
	/// Change handler will be passed dictionary with keys representing states that have changed.
	/// Dictionary bool value serves no purpose.
	/// </summary>
	/// <param name="subscriberId"></param>
	/// <param name="changeHandler"></param>
	void SubscribeToDataChanges(Guid subscriberId, Func<IDictionary<string, bool>, Task> changeHandler);

	/// <summary>
	/// Unsubscribe from event trigger.
	/// Typically this is called in Dispose event.
	/// </summary>
	/// <param name="subscriberId"></param>
	void UnsubscribeToDataChanges(Guid subscriberId);

	/// <summary>
	/// Trigger a change event to be called, assuring given key is included in change handler Dictionary.
	/// </summary>
	/// <param name="key"></param>
	Task TriggerChangeAsync(string? key = null);

	/// <summary>
	/// Use this method to group together multiple state changes and trigger event handlers when finished.
	/// </summary>
	/// <param name="changeHandler"></param>
	/// <returns></returns>
	Task ApplyChangesAsync(Func<Task> changeHandler);
}
