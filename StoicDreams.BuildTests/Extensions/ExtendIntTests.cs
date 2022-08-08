namespace StoicDreams.Core.Extensions;

public class ExtendIntTests : TestFramework
{
	[Theory]
	[InlineData("0", 0, 16)]
	[InlineData("00", 0, 16, 1)]
	[InlineData("000", 0, 8, 2)]
	[InlineData("0000", 0, 8, 3)]
	[InlineData("0", 0, 32)]
	[InlineData("00", 0, 32, 1)]
	[InlineData("0377", 255, 8, 3)]
	[InlineData("ff", 255, 16, 0)]
	[InlineData("ff", 255, 16, 1)]
	[InlineData("7v", 255, 32, 1)]
	[InlineData("003_", 255, 64, 3)]
	[InlineData("001þ", 255, 128, 3)]
	[InlineData("000ž", 255, 512, 3)]
	[InlineData("002ɺ", 2555, 1024, 3)]
	public void Verify_Int_To_Base(string expectedBase64, int input, int baseValue, int pad = 0)
	{
		Assert.Equal(expectedBase64, input.ToBaseEncode(baseValue, pad));
	}
}
