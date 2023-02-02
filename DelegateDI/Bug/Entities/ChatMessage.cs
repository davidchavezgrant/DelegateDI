using DelegateDI.Bug.Dtos;

#pragma warning disable CS8618
namespace DelegateDI.Bug.Entities;

internal sealed class ChatMessage: Entity
{
	private ChatMessage() {}

	private ChatMessage(Guid        id,
						AbstractChannel     channel,
						UserProfile sender,
						string      text,
						long        timeSent)
	{
		this.Id                = id;
		this.Channel           = channel;
		this.SenderId          = sender.Id;
		this.TimeSentNodaTicks = timeSent;
		this.Content           = text;
		this.Flagged		   = false; 
	}

	public long     TimeSentNodaTicks { get; private set; }
	public string   Content           { get; private set; }
	public Guid     SenderId          { get; private set; }
	public AbstractChannel  Channel              { get; private set; }
	
	public bool Flagged { get; set; }

	public static ChatMessage Create(AbstractChannel    room,
									 UserProfile sender,
									 string      text,
									 long        time) => new(Guid.NewGuid(),
															  room,
															  sender,
															  text,
															  time);

	public ChatMessageDto ToDto(string senderPublicKey) => new(this.Id,
										 this.Channel.Id,
										 this.SenderId,
										 this.TimeSentNodaTicks,
										 this.Content, senderPublicKey, false);
}