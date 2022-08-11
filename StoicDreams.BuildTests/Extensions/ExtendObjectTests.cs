using System.Diagnostics.CodeAnalysis;

namespace StoicDreams.Core.Extensions;

public class ExtendObjectTests : TestFramework
{
	[Theory]
	[MemberData(nameof(JsonConversionInput))]
	public void Verify_Json_Conversions<TInput>(TInput input)
	{
		string json = input.ToJson<TInput>();
		TInput? deserialized = json.FromJson<TInput>();
		Assert.NotNull(deserialized);
		Assert.Equal(input, deserialized);
	}

	[Theory]
	[MemberData(nameof(JsonConversionInput))]
	public void Verify_Json_Conversions_From_Default<TInput>(TInput input)
	{
		TInput deserialized = "".FromJson<TInput>(() => input);
		Assert.NotNull(deserialized);
		Assert.Equal(input, deserialized);
	}

	[Theory]
	[MemberData(nameof(ObjectConversionInput))]
	public void Verify_Object_With_WebEncryptedString_Conversions<TInput>(TInput input)
		where TInput : new()
	{
		string json = input.ConvertToWebEncryptedString();
		TInput deserialized = json.ConvertFromWebEncryptedString<TInput>();
		Assert.Equal(input, deserialized);
	}

	public static IEnumerable<object[]> JsonConversionInput()
	{
		yield return new object[] { "Hello World" };
		yield return new object[] { new Test() { Id = 3, Name = "Hello" } };
	}

	public static IEnumerable<object[]> ObjectConversionInput()
	{
		yield return new object[] { new Test() { Id = 2, Name = "Hello" } };
		yield return new object[] { new Test() { Id = 3, Name = "World" } };
	}
}

public class Test
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;

	public Test() { }

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		if (!(obj is Test instance)) { return false; }
		return Id == instance.Id && Name == instance.Name;
	}
	public override string ToString()
	{
		return $"{Id}:{Name}";
	}
	public override int GetHashCode()
	{
		return ToString().GetHashCode();
	}
}