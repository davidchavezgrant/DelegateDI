using System.Net.Mime;
using DelegateDI.Bug;
using DelegateDI.Bug.Dtos;


namespace DelegateDI;

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

	public static IEndpointRouteBuilder MapBylinesEndpoints(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(ChatRoutes.ADD_CONTACT,
						  async (NewRoomRequest request, IChatApiContract api) =>
						  {
							  await api.CreateRoom(request);
							  return Results.Ok();
						  });

		endpoints.MapGet(ChatRoutes.LIST_ROOMS_BY_PROFILE_ID,
						 async (Guid profileId, IChatApiContract api) =>
						 {
							 var result = await api.ListRoomsByProfileId(profileId);
							 return Results.Ok(result);
						 });

		endpoints.MapGet(ChatRoutes.LIST_CHANNELS_BY_PROFILE_ID,
						 async (Guid profileId, IChatApiContract api) =>
						 {
							 var result = await api.ListChannelsByProfileId(profileId);
							 return Results.Ok(result);
						 });

		endpoints.MapGet(ChatRoutes.GET_PROFILES_BY_ID,
						 async (IEnumerable<Guid> ids, IChatApiContract api) =>
						 {
							 var result = await api.GetProfilesById(ids);
							 return Results.Ok(result);
						 });

		endpoints.MapGet(ChatRoutes.GET_PROFILE + "/{id:guid}",
						 async (Guid id, IChatApiContract api) =>
						 {
							 var result = await api.GetProfileById(id);
							 return Results.Ok(result);
						 });

		endpoints.MapPost(ChatRoutes.UPSERT_PROFILE,
						  async (CreateProfileRequest req, IChatApiContract api) =>
						  {
							  var result = await api.UpsertProfile(req.WalletAddress, req.AppId);
							  return Results.Ok(result);
						  });

		return endpoints;
	}
}