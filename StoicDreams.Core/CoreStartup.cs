namespace StoicDreams.Core;

public static class CoreStartup
{
	public static IServiceCollection AddStoicDreamsCore(this IServiceCollection services)
	{
		services.AddTransient<IApiRequest, ApiRequest>();
		services.AddSingleton<IJsonConvert, JsonConvert>();
		return services;
	}
}
