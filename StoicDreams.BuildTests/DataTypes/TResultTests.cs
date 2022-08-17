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

	[Theory]
	[MemberData(nameof(ApiResponseToTResultDataInput))]
	public void Verify_ApiResponse_To_TResultData<TData>(TResult<TData> expectedTranslation, ApiResponse response)
	{
		TResult<TData> defaultItem = new();
		TResult<TData> translatedItem = response;
		Assert.NotEqual(defaultItem, translatedItem);
		Assert.Equal(expectedTranslation, translatedItem);
	}

	[Theory]
	[MemberData(nameof(ApiResponseToTResultInput))]
	public void Verify_ApiResponse_To_TResult(TResult expectedTranslation, ApiResponse response)
	{
		TResult defaultItem = new();
		TResult translatedItem = response;
		Assert.NotEqual(defaultItem, translatedItem);
		Assert.Equal(expectedTranslation, translatedItem);
	}

	public static IEnumerable<object[]> ApiResponseToTResultDataInput()
	{
		yield return new object[] { new TResult<string>() { Status = TResultStatus.Success, Result = "Hello World", Message = "Hello World" }, new ApiResponse() { Data = "Hello World", Result = ResponseResult.Success } };
		yield return new object[] { new TResult<string>() { Status = TResultStatus.Exception, Message = "Hello World" }, new ApiResponse() { Error = "Hello World", Result = ResponseResult.Fail } };
	}

	public static IEnumerable<object[]> ApiResponseToTResultInput()
	{
		yield return new object[] { new TResult() { Status = TResultStatus.Success, Message = "Hello World" }, new ApiResponse() { Data = "Hello World", Result = ResponseResult.Success } };
	}
}
