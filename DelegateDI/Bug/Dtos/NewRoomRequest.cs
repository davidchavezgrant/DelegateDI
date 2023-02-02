using System.Collections.Immutable;


namespace DelegateDI.Bug.Dtos;

public sealed record NewRoomRequest(IEnumerable<(Guid, Permission)> MembersAndPermissions,
									ChannelType                     Type,
									Guid                            ApplicationId,
									bool                            PublicChannel,
									string                          FirstMessage,
									Guid                            FirstMessageSender)
{
	public static NewRoomRequest Empty => new(ImmutableArray<(Guid, Permission)>.Empty,
											  ChannelType.DIRECTMESSAGE,
											  Guid.Empty,
											  false,
											  "",
											  Guid.Empty);
}