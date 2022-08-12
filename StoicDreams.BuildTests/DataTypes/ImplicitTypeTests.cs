namespace StoicDreams.Core.DataTypes;

public class ImplicitTypeTests : TestFramework
{
	[Theory]
	[InlineData(1)]
	public void Verify_Base_Overrides(int input)
	{
		ImplicitInt value = input;
		Assert.True(value.Equals(input));
		Assert.False(value.Equals(input - 1));
		Assert.Equal($"{input}", value.ToString());
	}

	[Theory]
	[InlineData(8, 9, 7)]
	[InlineData((byte)8, 9, 7)]
	[InlineData((char)8, 9, 7)]
	[InlineData((short)8, 9, 7)]
	[InlineData((long)8, 9, 7)]
	public void Verify_Conversions_With_Bit_AND<TBaseType>(TBaseType input, TBaseType bitEquals, TBaseType bitUnequals)
		where TBaseType : struct
	{
		ImplicitType<TBaseType> value = input;
		TBaseType converted = value;
		Assert.Equal(input, converted);

		ImplicitType<TBaseType> impEquals = bitEquals;
		ImplicitType<TBaseType> impUnequals = bitUnequals;
		ImplicitType<TBaseType> bEqual = value & impEquals;
		ImplicitType<TBaseType> bUnequal = value & impUnequals;
		int resultEquals = Convert.ToInt32(bEqual.Value);
		int resultUnequals = Convert.ToInt32(bUnequal.Value);
		Assert.NotEqual(0, resultEquals);
		Assert.Equal(0, resultUnequals);
	}

	[Theory]
	[InlineData(8, 9, 9)]
	[InlineData((byte)8, 3, 11)]
	[InlineData((char)8, 4, 12)]
	[InlineData((short)8, 7, 15)]
	[InlineData((long)8, 1, 9)]
	public void Verify_Conversions_With_Bit_OR<TBaseType>(TBaseType input, TBaseType bitOr, int bitResult)
		where TBaseType : struct
	{
		ImplicitType<TBaseType> value = input;
		TBaseType converted = value;
		Assert.Equal(input, converted);

		ImplicitType<TBaseType> impOr = bitOr;
		ImplicitType<TBaseType> impResult = value | impOr;
		int resultEquals = Convert.ToInt32(impResult.Value);
		Assert.Equal(bitResult, resultEquals);
	}

	[Theory]
	[InlineData(8, 9, 1)]
	[InlineData((byte)8, 3, 11)]
	[InlineData((char)8, 4, 12)]
	[InlineData((short)8, 7, 15)]
	[InlineData((long)8, 1, 9)]
	public void Verify_Conversions_With_Bit_XOR<TBaseType>(TBaseType input, TBaseType bitOr, int bitResult)
		where TBaseType : struct
	{
		ImplicitType<TBaseType> value = input;
		TBaseType converted = value;
		Assert.Equal(input, converted);

		ImplicitType<TBaseType> impOr = bitOr;
		ImplicitType<TBaseType> impResult = value ^ impOr;
		int resultEquals = Convert.ToInt32(impResult.Value);
		Assert.Equal(bitResult, resultEquals);
	}

	[Fact]
	public void Verify_ImplicitInt()
	{
		ImplicitInt value = 2;
		ImplicitInt bitValue = 3;
		ImplicitInt bitAnd = value & bitValue;
		ImplicitInt bitOr = value | bitValue;
		ImplicitInt bitXOR = value ^ bitValue;
		int check = value;
		Assert.Equal(2, bitAnd.Value);
		Assert.True(bitAnd == value);
		Assert.True(bitAnd != bitValue);
		Assert.Equal(3, bitOr.Value);
		Assert.Equal(1, bitXOR.Value);
	}

	[Fact]
	public void Verify_ImplicitByte()
	{
		ImplicitByte value = 2;
		ImplicitByte bitValue = 3;
		ImplicitByte bitAnd = value & bitValue;
		ImplicitByte bitOr = value | bitValue;
		ImplicitByte bitXOR = value ^ bitValue;
		byte check = value;
		Assert.Equal(2, bitAnd.Value);
		Assert.True(bitAnd == value);
		Assert.True(bitAnd != bitValue);
		Assert.Equal(3, bitOr.Value);
		Assert.Equal(1, bitXOR.Value);
	}

	[Fact]
	public void Verify_ImplicitShort()
	{
		ImplicitShort value = 2;
		ImplicitShort bitValue = 3;
		ImplicitShort bitAnd = value & bitValue;
		ImplicitShort bitOr = value | bitValue;
		ImplicitShort bitXOR = value ^ bitValue;
		short check = value;
		Assert.Equal(2, bitAnd.Value);
		Assert.True(bitAnd == value);
		Assert.True(bitAnd != bitValue);
		Assert.Equal(3, bitOr.Value);
		Assert.Equal(1, bitXOR.Value);
	}

	[Fact]
	public void Verify_ImplicitChar()
	{
		ImplicitChar value = (char)2;
		ImplicitChar bitValue = (char)3;
		ImplicitChar bitAnd = value & bitValue;
		ImplicitChar bitOr = value | bitValue;
		ImplicitChar bitXOR = value ^ bitValue;
		char check = value;
		Assert.Equal(2, bitAnd.Value);
		Assert.True(bitAnd == value);
		Assert.True(bitAnd != bitValue);
		Assert.Equal(3, bitOr.Value);
		Assert.Equal(1, bitXOR.Value);
	}

	[Fact]
	public void Verify_ImplicitLong()
	{
		ImplicitLong value = 2;
		ImplicitLong bitValue = 3;
		ImplicitLong bitAnd = value & bitValue;
		ImplicitLong bitOr = value | bitValue;
		ImplicitLong bitXOR = value ^ bitValue;
		long check = value;
		Assert.Equal(2, bitAnd.Value);
		Assert.True(bitAnd == value);
		Assert.True(bitAnd != bitValue);
		Assert.Equal(3, bitOr.Value);
		Assert.Equal(1, bitXOR.Value);
	}
}
