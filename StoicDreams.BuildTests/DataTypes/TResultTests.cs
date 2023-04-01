namespace StoicDreams.Core.DataTypes;

public class TResultTests : TestFramework
{
	[Theory]
	[InlineData(234)]
	public void Verify_TResult<T>(T expectedResult)
	{
		IActions actions = ArrangeUnitTest(new TResult<T>());

		actions.Act<TResult<T>>(a =>
		{
			a.Result = expectedResult;
			return a;
		});

		actions.Assert<TResult<T>>(a =>
		{
			Assert.False(a.IsOkay);
		});

		actions.Act<TResult<T>>(a =>
		{
			a.Status = TResultStatus.Success;
			return a;
		});

		actions.Assert<TResult<T>>(a =>
		{
			Assert.True(a.IsOkay);
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
		yield return new object[] { new TResult<string>() { Status = TResultStatus.Success, Result = "Hello World", Message = "Hello World" }, new ApiResponse() { Data = "\"Hello World\"", Result = ResponseResult.Success } };
	}

	public static IEnumerable<object[]> ApiResponseToTResultInput()
	{
		yield return new object[] { new TResult() { Status = TResultStatus.Success, Message = "Hello World" }, new ApiResponse() { Data = "Hello World", Result = ResponseResult.Success } };
		yield return new object[] { new TResult() { Status = TResultStatus.Exception, Message = "Hello World" }, new ApiResponse() { Error = "Hello World", Result = ResponseResult.Fail } };
		yield return new object[] { new TResult() { Status = TResultStatus.Success, Message = "Hello World" }, new ApiResponse() { Data = "\"Hello World\"", Result = ResponseResult.Success } };
	}
}
