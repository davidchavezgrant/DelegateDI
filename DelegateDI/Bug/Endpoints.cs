namespace DelegateDI.Bug;

internal static class Endpoints
{
	private const string UPSERT_PROFILE              = "/api/profile";
	private const string GET_PROFILE                 = "/api/profile";
	private const string ADD_CONTACT                 = "/api/contacts";
	private const string LIST_ROOMS_BY_PROFILE_ID    = "/api/rooms";
	private const string GET_PROFILES_BY_ID          = "/api/profiles";
	private const string LIST_CHANNELS_BY_PROFILE_ID = "/api/channels";

	public static IEndpointRouteBuilder MapBylinesEndpoints(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(Endpoints.ADD_CONTACT,
						  async (NewRoomRequest request, IChatApiContract api) =>
						  {
							  await api.CreateRoom(request);
							  return Results.Ok();
						  });

		endpoints.MapGet(Endpoints.LIST_ROOMS_BY_PROFILE_ID,
						 async (Guid profileId, IChatApiContract api) =>
						 {
							 var result = await api.ListRoomsByProfileId(profileId);
							 return Results.Ok(result);
						 });

		endpoints.MapGet(Endpoints.LIST_CHANNELS_BY_PROFILE_ID,
						 async (Guid profileId, IChatApiContract api) =>
						 {
							 var result = await api.ListChannelsByProfileId(profileId);
							 return Results.Ok(result);
						 });

		endpoints.MapGet(Endpoints.GET_PROFILES_BY_ID,
						 async (Guid[] ids, IChatApiContract api) =>
						 {
							 var result = await api.GetProfilesById(ids);
							 return Results.Ok(result);
						 });

		endpoints.MapGet(Endpoints.GET_PROFILE + "/{id:guid}",
						 async (Guid id, IChatApiContract api) =>
						 {
							 var result = await api.GetProfileById(id);
							 return Results.Ok(result);
						 });

		endpoints.MapPost(Endpoints.UPSERT_PROFILE,
						  async (CreateProfileRequest req, IChatApiContract api) =>
						  {
							  var result = await api.UpsertProfile(req.WalletAddress, req.AppId);
							  return Results.Ok(result);
						  });

		return endpoints;
	}
}