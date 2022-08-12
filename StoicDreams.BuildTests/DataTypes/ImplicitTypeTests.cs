namespace StoicDreams.Core.DataTypes;

public class ImplicitTypeTests : TestFramework
{
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
}
