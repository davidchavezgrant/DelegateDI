using System.Collections.Immutable;


namespace DelegateDI.Bug;

public sealed record NewRoomRequest(IEnumerable<(Guid, Permission)>? MembersAndPermissions, ChannelType Type,  Guid ApplicationId, bool PublicChannel = false, string? FirstMessage = null, Guid? FirstMessageSender = null)
{
	public static NewRoomRequest Empty => new(ImmutableArray<(Guid, Permission)>.Empty, ChannelType.DIRECTMESSAGE, Guid.Empty, false, null, null);
}