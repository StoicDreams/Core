using Newtonsoft.Json;
using RichardSzalay.MockHttp;

namespace StoicDreams.Core.Data;

public class ApiRequestTests : TestFramework
{
	[Fact]
	public void Verify_Get()
	{
		IActions<IApiRequest> actions = ArrangeApiTest();

		actions.Act(async a =>
		{
			return await a.Service.Get<MockData>("https://myfi.ws/mockurl");
		});

		actions.Assert(a =>
		{
			TResult<MockData> data = a.GetResult<TResult<MockData>>();
			Assert.NotNull(data.Result);
			Assert.Equal(86, data.Result.Id);
			Assert.Equal("Default", data.Result.Name);
		});
	}

	private IActions<IApiRequest> ArrangeApiTest() => ArrangeTest<IApiRequest>(options =>
	{
		options.AddService<HttpClient>(() =>
		{
			string json = JsonConvert.SerializeObject(new MockData());
			MockHttpMessageHandler mockHttp = new();
			mockHttp.When("https://myfi.ws/mockurl").Respond("application/json", json);
			return mockHttp.ToHttpClient();
		});
		options.AddService<IApiRequest, ApiRequest>();
	});

	public class MockData
	{
		public int Id { get; set; } = 86;
		public string Name { get; set; } = "Default";
	}
}
