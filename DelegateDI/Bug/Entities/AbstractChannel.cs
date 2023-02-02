namespace DelegateDI.Bug.Entities;

/// <summary>
/// This class is the most basic, abstract notion of a communication channel.
/// All other channels are derived from this class.
/// This was recently renamed from BaseChannel, and final name is still TBD.
/// </summary>
internal abstract class AbstractChannel: Entity
{
	public string? Name { get; set; }
	public Uri? ImageUri { get; set; }
	public Guid              ApplicationId { get; set; }
	public List<ChatMessage> Messages      { get; } = new();

	public List<UserChannelConfiguration> Members { get; set; } = new();

	public abstract void SendMessage(ChatMessage message, string senderPublicKey);
}