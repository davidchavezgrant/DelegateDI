using DelegateDI.Bug.Dtos;


namespace DelegateDI.Bug.Entities;

/// <summary>
/// This represents all communication channels which are not chat rooms.
/// Currently this is inherited by CommunityChannel and BroadcastChannel.
/// </summary>
internal abstract class Channel: AbstractChannel
{
	public bool       IsPublic       { get; set; }
	
	public abstract override void SendMessage(ChatMessage message, string senderPublicKey);
	public static async Task<ChannelDto> MakeDto(Channel channel, UserChannelConfiguration? askingProfileConfig, GetEnsDomain? getEnsDomain)
	{
		Dictionary<Guid, ChannelMember> channelMembers = new ();
		foreach (var member in channel.Members)
		{
			string? ensDomain = null;
			if (getEnsDomain is not null)
				ensDomain = await getEnsDomain(member.UserProfile.WalletAddress);
			
			var channelMember = new ChannelMember(member.UserProfile.ToDto(ensDomain, null), member.Permissions.Permission);
			channelMembers.Add(member.UserProfile.Id, channelMember);
		}
		var type = channel.GetChannelType();

		return new ChannelDto(channel.Id,channelMembers,type, askingProfileConfig?.NumberUnreadMessages ?? 0,channel.ApplicationId, channel.IsPublic, channel?.Name, channel!.ImageUri != null ? channel?.ImageUri.ToString() : null);
	}

	public ChannelType GetChannelType()
	{
		var type = this.GetType();
		if (type == typeof(BroadcastChannel))
			return ChannelType.BROADCAST;
		return ChannelType.COMMUNITY;
	}
}