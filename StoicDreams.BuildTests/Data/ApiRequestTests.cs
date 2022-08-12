using RichardSzalay.MockHttp;

namespace StoicDreams.Core.Data;

public class ApiRequestTests : TestFramework
{
	[Fact]
	public async void Verify_Get()
	{
		string response = await Json.SerializeAsync(new MockData());
		IActions<IApiRequest> actions = ArrangeApiTest(mock =>
		{
			mock.When(HttpMethod.Get, "https://myfi.ws/mockurl").Respond("application/mock", response);
		});

		actions.Act(async a =>
		{
			return await a.Service.GetAsync<MockData>("https://myfi.ws/mockurl");
		});

		actions.Assert(a =>
		{
			TResult<MockData> data = a.GetResult<TResult<MockData>>();
			Assert.NotNull(data.Result);
			Assert.Equal(86, data.Result.Id);
			Assert.Equal("Default", data.Result.Name);
		});
	}

	[Fact]
	public void Verify_Get_String()
	{
		IActions<IApiRequest> actions = ArrangeApiTest(mock =>
		{
			mock.When(HttpMethod.Get, "https://myfi.ws/mockurl").Respond("application/mock", "Some Text");
		});

		actions.Act(async a =>
		{
			return await a.Service.GetAsync<string>("https://myfi.ws/mockurl");
		});

		actions.Assert(a =>
		{
			TResult<string> data = a.GetResult<TResult<string>>();
			Assert.True(data.IsOkay);
			Assert.NotNull(data.Result);
			Assert.Equal("Some Text", data.Result);
		});
	}

	[Fact]
	public async Task Verify_Post()
	{
		MockData input = new() { Id = 3, Name = "This is input" };
		string response = await Json.SerializeAsync(new MockData());
		string expectedInput = await Json.SerializeAsync(input);
		IActions<IApiRequest> actions = ArrangeApiTest(mock =>
		{
			mock.When(HttpMethod.Post, "https://myfi.ws/mockurl").WithContent(expectedInput).Respond("application/mock", response);
		});

		actions.Act(async a =>
		{
			return await a.Service.PostJsonAsync<MockData, MockData>("https://myfi.ws/mockurl", input);
		});

		actions.Assert(a =>
		{
			TResult<MockData> data = a.GetResult<TResult<MockData>>();
			Assert.NotNull(data.Result);
			Assert.Equal(86, data.Result.Id);
			Assert.Equal("Default", data.Result.Name);
		});
	}

	private IJsonConvert Json => new JsonConvert();

	private IActions<IApiRequest> ArrangeApiTest(Action<MockHttpMessageHandler> mockSetup) => ArrangeTest<IApiRequest>(options =>
	{
		options.AddService<HttpClient>(() =>
		{
			MockHttpMessageHandler mockHttp = new();
			mockSetup.Invoke(mockHttp);
			return mockHttp.ToHttpClient();
		});
		options.AddService<IJsonConvert, JsonConvert>();
		options.AddService<IApiRequest, ApiRequest>();
	});

	public class MockData
	{
		public int Id { get; set; } = 86;
		public string Name { get; set; } = "Default";
	}
}
