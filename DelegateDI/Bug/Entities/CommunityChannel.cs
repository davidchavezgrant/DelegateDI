using DelegateDI.Bug.Events;


namespace DelegateDI.Bug.Entities;

internal class CommunityChannel: Channel
{
	
	private void CheckIfSenderHasWritePermissions(ChatMessage message)
	{
		if (this.IsPublic)
			return;

		if (this.Members.Any(x => x.UserProfile.Id == message.SenderId))
			return;

		throw new Exception("Sender does not have write permissions");
	}
	public override void SendMessage(ChatMessage message, string senderPublicKey)
	{
		this.CheckIfSenderHasWritePermissions(message);
		this.Messages.Add(message);
		var dto = message.ToDto(senderPublicKey);
		this.RaiseEvent(new MessageSentEvent(dto, this.Members.Select(p => p.UserProfile.WalletAddress).ToArray()));

	}
	public static async Task<CommunityChannel> Create(bool isPublic, List<UserChannelConfiguration> members, Guid app, GetEnsDomain? getEnsDomain)
	{
		var channel = new CommunityChannel
		{
			Id             = Guid.NewGuid(),
			IsPublic       = isPublic,
			ApplicationId = app,
			Members = members

		};
		var dto = await MakeDto(channel, null, getEnsDomain);
		channel.RaiseEvent(new ChannelCreatedEvent(dto));
		return channel;
	}
	
}