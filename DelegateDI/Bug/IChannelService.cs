using DelegateDI.Bug.Dtos;


namespace DelegateDI.Bug;

public interface IChannelService
{
	Task<Guid> CreateChannel(NewRoomRequest request, string? channelName);

	void GuardAgainstDuplicateMembers(NewRoomRequest request);

	Task<List<ChannelDto>> ListChannelsByApplicationId(Guid dAppId);

	Task UpdateChannel(Guid        channelId,
					   ChannelType channelType,
					   string?     newName,
					   Uri?        imageUri);

	Task             DeleteChannel(Guid      channelId,   ChannelType channelType);
	Task<Guid>       AddUserToChannel(Guid   profileId,   Guid        applicationId, string channelName);
	Task<ChannelDto> GetChannelByName(string channelName, Guid        applicationId);
}