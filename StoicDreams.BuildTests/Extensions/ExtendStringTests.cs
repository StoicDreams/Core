namespace StoicDreams.Core.Extensions;

public class ExtendStringTests : TestFramework
{
	[Theory]
	[InlineData("H****", "Hello")]
	[InlineData("Lon***end", "Longer text exposes start and end")]
	public void Verify_FilterValue(string filteredResult, string input)
	{
		Assert.Equal(filteredResult, input.FilterValue());
	}

	[Theory]
	[InlineData(TestEnum.One, "One")]
	[InlineData(TestEnum.One, "one")]
	[InlineData(TestEnum.One, "1")]
	[InlineData(TestEnum.Two, "TWO")]
	[InlineData(TestEnum.Two, "two")]
	[InlineData(TestEnum.Two, "2")]
	public void Verify_AsEnum(object result, string input)
	{
		Assert.Equal(result, input.AsEnum<TestEnum>());
	}
	public enum TestEnum
	{
		One = 1,
		Two = 2,
		Three = 3
	}

	[Theory]
	[InlineData(@"{}", "")]
	[InlineData(@"{
  ""Hello"": ""World""
}", "{\"Hello\":\"World\"}")]
	public void Verify_PrettifyJson(string prettyJson, string minimizedJson)
	{
		Assert.Equal(prettyJson, minimizedJson.PrettifyJson());
	}

	[Theory]
	[InlineData(0, "")]
	[InlineData(-327378614, "Hello")]
	[InlineData(-327419862, "hello")]
	[InlineData(217974194, "~)%!!#)*(")]
	[InlineData(946372051, "~)%!!#*(")]
	public void Verify_GetStaticHashCode(int hashCode, string input)
	{
		Assert.Equal(hashCode, input.GetStaticHashCode());
	}

	[Theory]
	[InlineData(@"C:\", "C:/")]
	[InlineData(@"C:\", "C:/root")]
	[InlineData(@"C:\root", @"C:\root\one")]
	[InlineData(@"Z:\root\one", "Z:/root/one/two")]
	public void Verify_GetParentFolder(string parentFolder, string pathInput)
	{
		Assert.Equal(parentFolder, pathInput.GetParentFolder());
	}

	[Theory]
	[InlineData(@"C:\one", "two", @"C:\one\two\three\four")]
	[InlineData(@"C:\one\two", "three", @"C:\one\two\three\four")]
	[InlineData(@"C:\one\two\three\four", "five", @"C:\one\two\three\four")]
	[InlineData(@"C:\", "one", @"C:\one\two\three\four\one\two")]
	[InlineData(@"C:\one\two\three\four", "one", @"C:\one\two\three\four\one\two", true)]
	public void Verify_BubbleUpToParentIfChildFolderExists(string expectedPath, string childFolder, string inputPath, bool stopTopMost = false)
	{
		Assert.Equal(expectedPath, inputPath.BubbleUpToParentIfChildFolderExists(childFolder, stopTopMost));
	}

	[Theory]
	[InlineData("", "")]
	[InlineData("&amp;", "&")]
	[InlineData("&lt;script /&gt;", "<script />")]
	public void Verify_HtmlEncode(string encoded, string input)
	{
		Assert.Equal(encoded, input.HtmlEncode());
	}

	[Theory]
	[InlineData("a")]
	[InlineData("This is some longer string")]
	public void Veify_Base64_UrlSafe_Conversions(string input)
	{
		string base64 = input.ToBase64();
		string urlSafe = base64.Base64ToUrlSafe();
		Assert.NotEqual(urlSafe, base64);
		string base64Returned = urlSafe.Base64FromUrlSafe();
		Assert.Equal(base64, base64Returned);
	}

	[Theory]
	[InlineData("Some input")]
	public void Verify_WebEncryptString_And_WebDecryptString(string input)
	{
		string encryptedA = input.WebEncryptString("one");
		Assert.NotEqual(encryptedA, input);
		string encryptedB = input.WebEncryptString("2");
		Assert.NotEqual(encryptedA, encryptedB);
		Assert.Equal(input, encryptedA.WebDecryptString("one"));
		Assert.Equal(input, encryptedB.WebDecryptString("2"));
		Assert.NotEqual(input, encryptedA.WebDecryptString("2"));
	}

	[Theory]
	[InlineData("m3HSJL1i83hdltRq0_o9czGb_8KJDKra4t-3JRlnPKcjI8PZm6XBHXx6zG4UuMXaDEZjR1wuXDre9G9zvN7AQw", "hello")]
	public void Verify_Hash(string hash, string input)
	{
		Assert.Equal(hash, input.Hash());
	}

	[Theory]
	[InlineData("", "")]
	[InlineData("This Is A Phrase", "ThisIsAPhrase")]
	public void Verify_PascalToSpaced(string expectedResult, string input)
	{
		IActions actions = ArrangeUnitTest(() => input);

		actions.Act((string value) => value.PascalToSpaced());

		actions.Assert((string? result) => result.IsNotNull().Should().Be(expectedResult));
	}

	[Theory]
	[InlineData(new string[0], "")]
	[InlineData(new string[] { "a" }, "a")]
	[InlineData(new string[] { "a", "b" }, "a\n\nb\n\n")]
	public void Verify_ToStringArray(string[] expectedResult, string input)
	{
		IActions actions = ArrangeUnitTest(() => input);

		actions.Act((string value) => value.ToStringArray());

		actions.Assert((string[]? result) => result.IsNotNull().Should().Equal(expectedResult));
	}
}
