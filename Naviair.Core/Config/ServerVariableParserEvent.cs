using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
public class ServerVariablesMiddleware
{
	private readonly RequestDelegate _next;

	public ServerVariablesMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		var serverVariables = new Dictionary<string, object>
		{
			{ "naviair", new Dictionary<string, string>
				{
					{ "naviairAuthorizedApi", "/api/naviairAuthorized" },
					{ "naviairApi", "/api/naviair" }
				}
			}
		};

		context.Items["Umbraco.ServerVariables"] = JsonConvert.SerializeObject(serverVariables);

		await _next(context);
	}
}

public static class ServerVariablesMiddlewareExtensions
{
	public static IApplicationBuilder UseServerVariablesMiddleware(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<ServerVariablesMiddleware>();
	}
}
