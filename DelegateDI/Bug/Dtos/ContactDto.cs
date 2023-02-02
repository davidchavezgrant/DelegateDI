namespace DelegateDI.Bug.Dtos;

public sealed record ContactDto(Guid   OwnerProfileId,
								Guid   ContactProfileId,
								string WalletAddress,
								string Nickname);