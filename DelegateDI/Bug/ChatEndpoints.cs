using System.Net.Mime;
using Bylines.Chat.Contracts;
using Bylines.Chat.Contracts.Dtos;
using DelegateDI.Bug.Dtos;


namespace DelegateDI.Bug;

internal static class ChatEndpoints
{
	public static IEndpointRouteBuilder MapChatEndpoints(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapPost(ChatRoutes.ADD_CONTACT,
						  async (NewRoomRequest request, IChatApiContract api) =>
						  {
							  await api.CreateRoom(request);
							  return Results.Ok();
						  })
				 .WithName("CreateTwoPersonRoomWith")
				 .WithGroupName("Contacts")
				 .Accepts<NewRoomRequest>(false, MediaTypeNames.Application.Json)
				 .Produces<IResult>();

		endpoints.MapGet(ChatRoutes.LIST_ROOMS_BY_PROFILE_ID,
					     async (Guid profileId, IChatApiContract api) =>
					     {
						     var result = await api.ListRoomsByProfileId(profileId);
						     return Results.Ok(result);
					     })
				 .WithName("ListRoomsByProfileId")
				 .WithGroupName("Rooms")
				 .Accepts<Guid>(false, MediaTypeNames.Application.Json)
				 .Produces<IEnumerable<DirectMessageDto>>();

		endpoints.MapGet(ChatRoutes.LIST_CHANNELS_BY_PROFILE_ID,
					     async (Guid profileId, IChatApiContract api) =>
					     {
						     var result = await api.ListChannelsByProfileId(profileId);
						     return Results.Ok(result);
					     })
				 .WithName("ListChannelsByProfileId")
				 .WithGroupName("Channels")
				 .Accepts<Guid>(false, MediaTypeNames.Application.Json)
				 .Produces<IEnumerable<ChannelDto>>();

		return endpoints;
	}
}