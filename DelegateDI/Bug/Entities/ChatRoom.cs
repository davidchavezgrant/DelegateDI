using DelegateDI.Bug.Dtos;
using DelegateDI.Bug.Events;

#pragma warning disable CS8618
namespace DelegateDI.Bug.Entities;

/// <summary>
/// This represents a one-to-one chat room (direct message)
/// </summary>
internal sealed class ChatRoom: AbstractChannel
{
	private ChatRoom() {}
	
	public static ChatRoom Create(Guid appId, List<UserChannelConfiguration> members)
	{
		var newRoom = new ChatRoom
		{
			Members = members,
			ApplicationId = appId,
			Id            = Guid.NewGuid()
		};
		
		return newRoom;
	}
	
	public static async Task<DirectMessageDto> MakeDto(ChatRoom                     room,
											  int                                   numberUnreadMessages,
											  GetEnsDomain?                         getEnsDomain,
											  GetEncryptionPublicKeyByWalletAddress getEncryptionPublicKey)
	{
		Dictionary<Guid, ProfileDto> roomMembers = new ();
		foreach (var member in room.Members)
		{
			string? publicKey = await getEncryptionPublicKey(member.UserProfile.WalletAddress);
			string? ensDomain = await getEnsDomain(member.UserProfile.WalletAddress);
			roomMembers.Add(member.UserProfile.Id, member.UserProfile.ToDto(ensDomain, publicKey));
		}
		
		return room.ToDto() with
		{
			Members = roomMembers,
			NumberUnreadMessages = numberUnreadMessages
		};
	}

	public async Task PushRoom(ChatMessage? firstMessage, string senderPublicKey, GetEnsDomain? getEnsDomain, GetEncryptionPublicKeyByWalletAddress getEncryptionPublicKey)
	{
		var             roomDto    = await MakeDto(this, 0, getEnsDomain, getEncryptionPublicKey);

		ChatMessageDto? messageDto = null;
		if (firstMessage is not null)
		{
			this.Messages.Add(firstMessage);
			messageDto = firstMessage.ToDto(senderPublicKey);
		}
		this.RaiseEvent(new RoomCreatedEvent(roomDto, messageDto));
	}

	public override void SendMessage(ChatMessage message, string senderPublicKey)
	{
		if (!this.Members.Any(p => p.UserProfile.Id == message.SenderId))
			throw new ArgumentException($"Message sender is not a member of the room: {message.SenderId}");
		
		
		this.Messages.Add(message);
		var dto = message.ToDto(senderPublicKey);
		this.RaiseEvent(new MessageSentEvent(dto, this.Members.Select(p => p.UserProfile.WalletAddress).ToArray()));
	}

	public DirectMessageDto ToDto()
	{
		var members = new Dictionary<Guid, ProfileDto>();
		//foreach (var member in this.Owner.Members) members.Add(member.Id, member.DisplayName ?? member.WalletAddress);
		return new DirectMessageDto(this.Id, members, 0, this.ApplicationId, null, this.Name);
	}
}