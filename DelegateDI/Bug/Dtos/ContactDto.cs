namespace Bylines.Chat.Contracts.Dtos;

public sealed record ContactDto(Guid   OwnerProfileId,
								Guid   ContactProfileId,
								string WalletAddress,
								string Nickname);