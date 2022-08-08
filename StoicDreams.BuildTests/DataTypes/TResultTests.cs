namespace StoicDreams.Core.DataTypes;

public class TResultTests : TestFramework
{
	[Theory]
	[InlineData(234)]
	public void Verify_TResult<T>(T expectedResult)
	{
		IActions<TResult<T>> actions = ArrangeTest<TResult<T>>(options =>
		{
			TResult<T> result = new();
			options.AddService(() => result);
		});

		actions.Act(a =>
		{
			a.Service.Result = expectedResult;
		});

		actions.Assert(a =>
		{
			Assert.False(a.Service.IsOkay);
		});

		actions.Act(a =>
		{
			a.Service.Status = TResultStatus.Success;
		});

		actions.Assert(a =>
		{
			Assert.True(a.Service.IsOkay);
		});
	}
}