namespace DelegateDI.Bug;

public delegate Task<string?> GetEncryptionPublicKeyByWalletAddress(string walletAddress);

public delegate Task<string?> GetEnsDomain(string walletAddress);

internal interface IChatApiContract
{
	Task<Guid>                          CreateRoom(NewRoomRequest         request);
	Task<ProfileDto>                    GetProfileById(Guid               profileId);
	Task<IEnumerable<ProfileDto>>       GetProfilesById(IEnumerable<Guid> ids);
	Task<IEnumerable<ChannelDto>>       ListChannelsByProfileId(Guid      profileId);
	Task<IEnumerable<DirectMessageDto>> ListRoomsByProfileId(Guid         profileId);
	Task<Guid>                          UpsertProfile(string              walletAddress, Guid appId);
}

internal interface IChannelService
{
	Task<Guid> CreateChannel(NewRoomRequest request, string? channelName);

	void GuardAgainstDuplicateMembers(NewRoomRequest request);
}