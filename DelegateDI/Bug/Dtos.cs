using DelegateDI.Bug.Entities;


namespace DelegateDI.Bug;

internal sealed record ChannelDto(Guid                            ChannelId,
								  Dictionary<Guid, ChannelMember> Members,
								  ChannelType                     Type,
								  int                             NumberUnreadMessages,
								  Guid                            ApplicationId,
								  bool                            IsPublic,
								  string?                         Name,
								  string?                         Image = null);

internal sealed record ChatMessageDto(Guid   Id,
									  Guid   RoomId,
									  Guid   SenderId,
									  long   InstantSent,
									  string CipherText,
									  string SenderEncryptionPublicKey,
									  bool   PushedToClient,
									  bool   IsFromMe = false,
									  Uri?   ImageUri = null,
									  bool   Flagged  = false);

internal sealed record ContactDto(Guid   OwnerProfileId,
								  Guid   ContactProfileId,
								  string WalletAddress,
								  string Nickname);

internal sealed record CreateProfileRequest(Guid AppId, string WalletAddress);

internal sealed record DirectMessageDto(Guid                         RoomId,
										Dictionary<Guid, ProfileDto> Members,
										int                          NumberUnreadMessages,
										Guid                         ApplicationId,
										Uri?                         Image    = null,
										string?                      RoomName = null);

internal sealed record EncryptedPrivateKey(string? Version,
										   string? Nonce,
										   string? EphemPublicKey,
										   string? Ciphertext);

internal sealed record NewRoomRequest(IEnumerable<(Guid, Permission)> MembersAndPermissions,
									  ChannelType                     Type,
									  Guid                            ApplicationId,
									  bool                            PublicChannel,
									  string                          FirstMessage,
									  Guid                            FirstMessageSender);

internal sealed record ProfileDto(Guid            ProfileId,
								  string          WalletAddress,
								  string          DisplayName,
								  string          EnsDomain,
								  string          EncryptionPublicKey,
								  string          Email,
								  Uri?            AvatarUri   = null,
								  UserPermissions Permissions = UserPermissions.NONE);