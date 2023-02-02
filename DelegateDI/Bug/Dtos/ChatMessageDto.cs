
namespace DelegateDI.Bug.Dtos;

public sealed record ChatMessageDto(Guid Id, Guid RoomId, Guid SenderId, long InstantSent, string CipherText, string SenderEncryptionPublicKey, bool PushedToClient, bool IsFromMe = false, Uri? ImageUri = null, bool Flagged = false)
{
	public static ChatMessageDto Empty=> new(Guid.Empty, Guid.Empty, Guid.Empty, 0, "hello", "eipj", false, false, null, false);
}