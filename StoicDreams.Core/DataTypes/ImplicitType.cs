namespace StoicDreams.Core.DataTypes;

public class ImplicitType<TBaseType>
	where TBaseType : struct
{
	public TBaseType Value { get; set; } = default;

	public static implicit operator TBaseType(ImplicitType<TBaseType> input) => input.Value;
	public static implicit operator ImplicitType<TBaseType>(TBaseType input) => new() { Value = input };

	public override bool Equals(object? obj)
	{
		if (obj is ImplicitType<TBaseType> impValue) { return Value.Equals(impValue.Value); }
		if (obj is TBaseType value) { return Value.Equals(value); }
		return false;
	}
	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}

	public override string ToString()
	{
		return $"{Value}";
	}

	public static bool operator ==(ImplicitType<TBaseType> input, ImplicitType<TBaseType> other)
	{
		{ if (input.Value is int a && other.Value is int b) { return a == b; } }
		{ if (input.Value is byte a && other.Value is byte b) { return a == b; } }
		{ if (input.Value is char a && other.Value is char b) { return a == b; } }
		{ if (input.Value is short a && other.Value is short b) { return a == b; } }
		{ if (input.Value is long a && other.Value is long b) { return a == b; } }
		throw new NotImplementedException("Unexpected type used for bit AND conversion");
	}
	public static bool operator !=(ImplicitType<TBaseType> input, ImplicitType<TBaseType> other)
	{
		{ if (input.Value is int a && other.Value is int b) { return a != b; } }
		{ if (input.Value is byte a && other.Value is byte b) { return a != b; } }
		{ if (input.Value is char a && other.Value is char b) { return a != b; } }
		{ if (input.Value is short a && other.Value is short b) { return a != b; } }
		{ if (input.Value is long a && other.Value is long b) { return a != b; } }
		throw new NotImplementedException("Unexpected type used for bit AND conversion");
	}

	public static ImplicitType<TBaseType> operator &(ImplicitType<TBaseType> input, ImplicitType<TBaseType> other)
	{
		{ if (input.Value is int a && other.Value is int b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(a & b); } }
		{ if (input.Value is byte a && other.Value is byte b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(byte)(a & b); } }
		{ if (input.Value is char a && other.Value is char b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(char)(a & b); } }
		{ if (input.Value is short a && other.Value is short b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(short)(a & b); } }
		{ if (input.Value is long a && other.Value is long b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(a & b); } }
		throw new NotImplementedException("Unexpected type used for bit AND conversion");
	}
	public static ImplicitType<TBaseType> operator |(ImplicitType<TBaseType> input, ImplicitType<TBaseType> other)
	{
		{ if (input.Value is int a && other.Value is int b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(a | b); } }
		{ if (input.Value is byte a && other.Value is byte b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(byte)(a | b); } }
		{ if (input.Value is char a && other.Value is char b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(char)(a | b); } }
		{ if (input.Value is short a && other.Value is short b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(short)(a | b); } }
		{ if (input.Value is long a && other.Value is long b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(a | b); } }
		throw new NotImplementedException("Unexpected type used for bit AND conversion");
	}
	public static ImplicitType<TBaseType> operator ^(ImplicitType<TBaseType> input, ImplicitType<TBaseType> other)
	{
		{ if (input.Value is int a && other.Value is int b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(a ^ b); } }
		{ if (input.Value is byte a && other.Value is byte b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(byte)(a ^ b); } }
		{ if (input.Value is char a && other.Value is char b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(char)(a ^ b); } }
		{ if (input.Value is short a && other.Value is short b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(short)(a ^ b); } }
		{ if (input.Value is long a && other.Value is long b) { return (ImplicitType<TBaseType>)(TBaseType)(object)(a ^ b); } }
		throw new NotImplementedException("Unexpected type used for bit AND conversion");
	}
}

public class ImplicitInt : ImplicitType<int>
{
	public static implicit operator int(ImplicitInt input) => input.Value;
	public static implicit operator ImplicitInt(int input) => new() { Value = input };
	public static ImplicitInt operator &(ImplicitInt input, ImplicitInt other) => new() { Value = input.Value & other.Value };
	public static ImplicitInt operator |(ImplicitInt input, ImplicitInt other) => new() { Value = input.Value | other.Value };
	public static ImplicitInt operator ^(ImplicitInt input, ImplicitInt other) => new() { Value = input.Value ^ other.Value };
}
public class ImplicitByte : ImplicitType<byte>
{
	public static implicit operator byte(ImplicitByte input) => input.Value;
	public static implicit operator ImplicitByte(byte input) => new() { Value = input };
	public static ImplicitByte operator &(ImplicitByte input, ImplicitByte other) => new() { Value = (byte)(input.Value & other.Value) };
	public static ImplicitByte operator |(ImplicitByte input, ImplicitByte other) => new() { Value = (byte)(input.Value | other.Value) };
	public static ImplicitByte operator ^(ImplicitByte input, ImplicitByte other) => new() { Value = (byte)(input.Value ^ other.Value) };
}
public class ImplicitChar : ImplicitType<char>
{
	public static implicit operator char(ImplicitChar input) => input.Value;
	public static implicit operator ImplicitChar(char input) => new() { Value = input };
	public static ImplicitChar operator &(ImplicitChar input, ImplicitChar other) => new() { Value = (char)(input.Value & other.Value) };
	public static ImplicitChar operator |(ImplicitChar input, ImplicitChar other) => new() { Value = (char)(input.Value | other.Value) };
	public static ImplicitChar operator ^(ImplicitChar input, ImplicitChar other) => new() { Value = (char)(input.Value ^ other.Value) };
}
public class ImplicitShort : ImplicitType<short>
{
	public static implicit operator short(ImplicitShort input) => input.Value;
	public static implicit operator ImplicitShort(short input) => new() { Value = input };
	public static ImplicitShort operator &(ImplicitShort input, ImplicitShort other) => new() { Value = (short)(input.Value & other.Value) };
	public static ImplicitShort operator |(ImplicitShort input, ImplicitShort other) => new() { Value = (short)(input.Value | other.Value) };
	public static ImplicitShort operator ^(ImplicitShort input, ImplicitShort other) => new() { Value = (short)(input.Value ^ other.Value) };
}
public class ImplicitLong : ImplicitType<long>
{
	public static implicit operator long(ImplicitLong input) => input.Value;
	public static implicit operator ImplicitLong(long input) => new() { Value = input };
	public static ImplicitLong operator &(ImplicitLong input, ImplicitLong other) => new() { Value = input.Value & other.Value };
	public static ImplicitLong operator |(ImplicitLong input, ImplicitLong other) => new() { Value = input.Value | other.Value };
	public static ImplicitLong operator ^(ImplicitLong input, ImplicitLong other) => new() { Value = input.Value ^ other.Value };
}
