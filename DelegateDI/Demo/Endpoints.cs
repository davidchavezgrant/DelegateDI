namespace DelegateDI.Demo;

internal static class Endpoints
{
	public static IEndpointRouteBuilder MapDemoEndpoints(this IEndpointRouteBuilder app)
	{
		app.MapGet("/singleton-service",
				   (MySingletonService service) =>
				   {
					   service.Log();
					   return $"{nameof(MySingletonService)} {service.Id}";
				   });
		app.MapGet("/singleton-delegate",
				   (MySingletonDelegate handle) =>
				   {
					   var handlerId = handle();
					   return $"{nameof(MySingletonDelegate)} {handlerId}";
				   });
		app.MapGet("/scoped-service",
				   (MyScopedService service) =>
				   {
					   service.Log();
					   return $"{nameof(MyScopedService)} {service.Id}";
				   });
		app.MapGet("/scoped-delegate",
				   (MyScopedDelegate handle) =>
				   {
					   var handlerId = handle();
					   return $"{nameof(MyScopedDelegate)} {handlerId}";
				   });
		app.MapGet("/transient-service",
				   (MyTransientService service) =>
				   {
					   service.Log();
					   return $"{nameof(MyTransientService)} {service.Id}";
				   });
		app.MapGet("/transient-delegate",
				   (MyTransientDelegate handle) =>
				   {
					   var handlerId = handle();
					   return $"{nameof(MyTransientDelegate)} {handlerId}";
				   });
		return app;
	}
}