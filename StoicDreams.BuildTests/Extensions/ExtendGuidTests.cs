namespace StoicDreams.Core.Extensions;

public class ExtendGuidTests : TestFramework
{
	[Theory]
	[InlineData("0000", "00000000-0000-0000-0000-000000000000")]
	[InlineData("66602a8941eaeb7570006c8f18f67b7a", "66602a89-eb75-41ea-8f6c-00707b7bf698")]
	public void Verify_ToBaseEncode(string encoded, string value)
	{
		Guid parsedGuid = Guid.Parse(value);
		Assert.Equal(encoded, parsedGuid.ToBaseEncode());
	}
}
