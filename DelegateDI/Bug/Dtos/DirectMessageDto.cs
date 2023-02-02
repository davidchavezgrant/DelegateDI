


namespace DelegateDI.Bug.Dtos;

public sealed record NewRoomDto(DirectMessageDto DirectMessage, ChatMessageDto? FirstMessage);

public sealed record DirectMessageDto(Guid                         RoomId,
							 Dictionary<Guid, ProfileDto> Members,
							 int                         NumberUnreadMessages,
							 Guid                         ApplicationId,
							 Uri?                         Image    = null,
							 string?                       RoomName = null)
{
	public string GetDefaultName(Guid currentProfileId)
	{
		if (!string.IsNullOrWhiteSpace(this.RoomName))
			return this.RoomName;
		var otherIds   = this.Members.Keys.Where(x => x != currentProfileId);
		var otherMembers = otherIds.Select(x => this.Members[x]).ToList();
		var otherNames = otherMembers.Select(m => !string.IsNullOrEmpty(m.DisplayName) ? m.DisplayName : !string.IsNullOrEmpty(m.EnsDomain) ?  m.EnsDomain : m.WalletAddress).ToList();
		return string.Join(", ", otherNames);
	}

	public static DirectMessageDto Empty => new(Guid.Empty, new Dictionary<Guid, ProfileDto>(), 0, Guid.Empty, null);

}
public static class ChannelAndRoomExtensions
{
	public static DirectMessageDto ToRoomDto(this ChannelDto channel)
	{
		return new DirectMessageDto(channel.ChannelId,
						   channel.Members.Select(m => new KeyValuePair<Guid,ProfileDto>(m.Key, m.Value.Profile)).ToDictionary( x => x.Key, x => x.Value),
						   channel.NumberUnreadMessages, 
						   channel.ApplicationId, 
						   null, 
						   channel.Name
						  );
	}
}