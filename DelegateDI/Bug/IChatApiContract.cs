using DelegateDI.Bug.Dtos;


namespace DelegateDI.Bug;

/// <summary>
/// This contract is implemented by the server side chat service (chatservice.cs) which the chat client ( ChatClient.cs ) calls.
/// The contract is also implemented by IChatHttpContract, which allows making the same requests over http.
/// </summary>
public interface IChatApiContract
{
	Task<Guid>                          CreateRoom(NewRoomRequest    request);
	Task<IEnumerable<ChannelDto>>       ListChannelsByProfileId(Guid profileId);
	Task<IEnumerable<DirectMessageDto>> ListRoomsByProfileId(Guid    profileId);
}