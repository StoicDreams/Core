using RichardSzalay.MockHttp;
using System.Text.Json.Serialization;

namespace StoicDreams.Core.Data;

public class ApiRequestTests : TestFramework
{
	[Fact]
	public async void Verify_Get_ApiResponse()
	{
		string response = await Json.SerializeAsync(new ApiResponse() { Result = ResponseResult.Success, Data = "Hello World"});
		IActions<IApiRequest> actions = ArrangeApiTest(mock =>
		{
			mock.When(HttpMethod.Get, "https://myfi.ws/mockurl").Respond("application/mock", response);
		});

		actions.Act(async a =>
		{
			return await a.Service.GetAsync<string>("https://myfi.ws/mockurl");
		});

		actions.Assert(a =>
		{
			TResult<string> data = a.GetResult<TResult<string>>();
			Assert.NotNull(data.Result);
			Assert.Equal("Hello World", data.Result);
		});
	}

	[Fact]
	public async void Verify_Get_ApiResponse_As_TResult()
	{
		string response = await Json.SerializeAsync(new ApiResponse() { Result = ResponseResult.Success, Data = "Hello World" });
		IActions<IApiRequest> actions = ArrangeApiTest(mock =>
		{
			mock.When(HttpMethod.Get, "https://myfi.ws/mockurl").Respond("application/mock", response);
		});

		actions.Act(async a =>
		{
			return (TResult)(await a.Service.GetAsync<string>("https://myfi.ws/mockurl"));
		});

		actions.Assert(a =>
		{
			TResult data = a.GetResult<TResult>();
			Assert.NotNull(data.Message);
			Assert.Equal("Hello World", data.Message);
		});
	}

	[Fact]
	public async void Verify_Get_ApiResponse_with_MockData()
	{
		string response = await Json.SerializeAsync(new ApiResponse() { Result = ResponseResult.Success, Data = new MockData() { Id = 27, Name = "Blerp!" } });
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
			Assert.Equal(27, data.Result.Id);
			Assert.Equal("Blerp!", data.Result.Name);
		});
	}

	[Fact]
	public void Verify_Get_ApiResponse_with_LoginInfo()
	{
		Guid guid = Guid.NewGuid();
		string response = $"{{\"result\":1,\"data\":{{\"userID\":\"{guid}\",\"displayName\":\"Timmy\",\"email\":\"mockemail@myfi.ws\",\"createdDate\":\"2018-09-01T05:30:05.3181046Z\"}}}}";
		IActions<IApiRequest> actions = ArrangeApiTest(mock =>
		{
			mock.When(HttpMethod.Get, "https://myfi.ws/mockurl").Respond("application/mock", response);
		});

		actions.Act(async a =>
		{
			return await a.Service.GetAsync<LoginInfo>("https://myfi.ws/mockurl");
		});

		actions.Assert(a =>
		{
			TResult<LoginInfo> data = a.GetResult<TResult<LoginInfo>>();
			Assert.NotNull(data.Result);
			Assert.Equal("mockemail@myfi.ws", data.Result.Email);
			Assert.Equal("Timmy", data.Result.DisplayName);
			Assert.Equal(guid, data.Result.UserId);
		});
	}

	[Fact]
	public void Verify_Get_ApiResponse_with_Failed_LoginInfo()
	{
		string response = $"{{\"result\":0}}";
		IActions<IApiRequest> actions = ArrangeApiTest(mock =>
		{
			mock.When(HttpMethod.Get, "https://myfi.ws/mockurl").Respond("application/mock", response);
		});

		actions.Act(async a =>
		{
			return await a.Service.GetAsync<LoginInfo>("https://myfi.ws/mockurl");
		});

		actions.Assert(a =>
		{
			TResult<LoginInfo> data = a.GetResult<TResult<LoginInfo>>();
			Assert.Null(data.Result);
			Assert.False(data.IsOkay);
		});
	}

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

	[Fact]
	public async Task Verify_Put()
	{
		MockData input = new() { Id = 3, Name = "This is input" };
		string response = await Json.SerializeAsync(new MockData());
		string expectedInput = await Json.SerializeAsync(input);
		IActions<IApiRequest> actions = ArrangeApiTest(mock =>
		{
			mock.When(HttpMethod.Put, "https://myfi.ws/mockurl").WithContent(expectedInput).Respond("application/mock", response);
		});

		actions.Act(async a =>
		{
			return await a.Service.PutJsonAsync<MockData, MockData>("https://myfi.ws/mockurl", input);
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
	public async void Verify_Delete()
	{
		string response = await Json.SerializeAsync(new MockData());
		IActions<IApiRequest> actions = ArrangeApiTest(mock =>
		{
			mock.When(HttpMethod.Delete, "https://myfi.ws/mockurl").Respond("application/mock", response);
		});

		actions.Act(async a =>
		{
			return await a.Service.DeleteAsync<MockData>("https://myfi.ws/mockurl");
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

	internal class LoginInfo
	{
		[JsonPropertyName("createdDate")]
		public DateTime CreatedDate { get; set; }

		[JsonPropertyName("displayName")]
		public string DisplayName { get; set; } = "User";

		[JsonPropertyName("email")]
		public string Email { get; set; } = string.Empty;

		[JsonPropertyName("roles")]
		public int Roles { get; set; } = 1;

		[JsonPropertyName("userID")]
		public Guid UserId { get; set; }
	}

}
