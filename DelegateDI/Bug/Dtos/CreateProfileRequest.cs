namespace DelegateDI.Bug.Dtos;

internal sealed record CreateProfileRequest(Guid AppId, string WalletAddress);