namespace DelegateDI.Bug.Dtos;

public sealed record ChannelDto(Guid                            ChannelId,
								Dictionary<Guid, ChannelMember> Members,
								ChannelType                     Type,
								int                             NumberUnreadMessages,
								Guid                            ApplicationId,
								bool                            IsPublic,
								string?                         Name,
								string?                         Image = null);
public sealed record ChannelMember(ProfileDto Profile, Permission Permission);
public delegate Task<string?> GetEncryptionPublicKeyByWalletAddress(string walletAddress);

